using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using QFRMS.Data.DTOs;
using QFRMS.Data.Interfaces;
using QFRMS.Data.Models;
using QFRMS.Data.ViewModels;
using static QFRMS.Data.Enums.EnumHelper;
using QFRMS.Services.Interfaces;
using QFRMS.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QFRMS.Data.Constants;
using Microsoft.AspNetCore.Mvc;

namespace QFRMS.Services.Services
{
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository _repository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserAccountRepository _userRepository;
        private readonly IPDFRepository _pdfRepository;
        private readonly ILogger<BatchService> _logger;
        private readonly Work _work = new Work();

        public BatchService(IBatchRepository repository, ICourseRepository courseRepository, IUserAccountRepository userAccountRepository, IPDFRepository pdfRepository, ILogger<BatchService> logger)
        {
            _repository = repository;
            _courseRepository = courseRepository;
            _userRepository = userAccountRepository;
            _pdfRepository = pdfRepository;
            _logger = logger;
        }

        public async Task<IQueryable<BatchListViewModel>> GetBatchListAsync()
        {
            try
            {
                return await Task.FromResult(from batch in await _repository.RetrieveAllAsync()
                                             join course in await _courseRepository.RetrieveAllAsync() on batch.CourseId equals course.Id
                                             join trainor in await _userRepository.GetUsersAsync() on batch.TrainorId equals trainor.Id
                                             orderby batch.RQMNumber, course.ProgramTitle, trainor.FirstName
                                             select new BatchListViewModel
                                             {
                                                 Id = batch.Id,
                                                 RQMCode = batch.RQMNumber,
                                                 ProgramTitle = course.ProgramTitle,
                                                 TrainorName = $"{trainor.FirstName} {trainor.MiddleName![0]} {trainor.LastName} {trainor.ExtensionName}",
                                                 Period = $"{batch.DateStart:MM/dd/yyyy} - {(batch.DateEnd.HasValue ? batch.DateEnd.Value.ToString("MM/dd/yyyy") : "TBA")}"
                                             });
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime} GetBatchListAsync Failed: {message}", DateTime.Now.ToString(), ex.Message);
                throw;
            }
        }

        public async Task<IQueryable<BatchListViewModel>> SearchBatchListAsync(string searchType, string searchInput)
        {
            try
            {
                return searchType switch
                {
                    "RQM" => from batch in await _repository.RetrieveAllAsync()
                             join course in await _courseRepository.RetrieveAllAsync() on batch.CourseId equals course.Id
                             join trainor in await _userRepository.GetUsersAsync() on batch.TrainorId equals trainor.Id
                             orderby batch.RQMNumber, course.ProgramTitle, trainor.FirstName
                             where batch.RQMNumber.Contains(searchInput)
                             select new BatchListViewModel
                             {
                                 Id = batch.Id,
                                 RQMCode = batch.RQMNumber,
                                 ProgramTitle = course.ProgramTitle,
                                 TrainorName = $"{trainor.FirstName} {trainor.MiddleName![0]} {trainor.LastName} {trainor.ExtensionName}",
                                 Period = $"{batch.DateStart:MM/dd/yyyy} - {(batch.DateEnd.HasValue ? batch.DateEnd.Value.ToString("MM/dd/yyyy") : "TBA")}"
                             },

                    "Title" => from batch in await _repository.RetrieveAllAsync()
                               join course in await _courseRepository.RetrieveAllAsync() on batch.CourseId equals course.Id
                               join trainor in await _userRepository.GetUsersAsync() on batch.TrainorId equals trainor.Id
                               orderby batch.RQMNumber, course.ProgramTitle, trainor.FirstName
                               where course.ProgramTitle.Contains(searchInput)
                               select new BatchListViewModel
                               {
                                   Id = batch.Id,
                                   RQMCode = batch.RQMNumber,
                                   ProgramTitle = course.ProgramTitle,
                                   TrainorName = $"{trainor.FirstName} {trainor.MiddleName![0]} {trainor.LastName} {trainor.ExtensionName}",
                                   Period = $"{batch.DateStart:MM/dd/yyyy} - {(batch.DateEnd.HasValue ? batch.DateEnd.Value.ToString("MM/dd/yyyy") : "TBA")}"
                               },

                    "Trainor" => from batch in await _repository.RetrieveAllAsync()
                                 join course in await _courseRepository.RetrieveAllAsync() on batch.CourseId equals course.Id
                                 join trainor in await _userRepository.GetUsersAsync() on batch.TrainorId equals trainor.Id
                                 orderby batch.RQMNumber, course.ProgramTitle, trainor.FirstName
                                 where
                                    trainor.FirstName!.Contains(searchInput) ||
                                    trainor.MiddleName!.Contains(searchInput) ||
                                    trainor.LastName!.Contains(searchInput) ||
                                    trainor.ExtensionName!.Contains(searchInput)
                                 select new BatchListViewModel
                                 {
                                     Id = batch.Id,
                                     RQMCode = batch.RQMNumber,
                                     ProgramTitle = course.ProgramTitle,
                                     TrainorName = $"{trainor.FirstName} {trainor.MiddleName![0]} {trainor.LastName} {trainor.ExtensionName}",
                                     Period = $"{batch.DateStart:MM/dd/yyyy} - {(batch.DateEnd.HasValue ? batch.DateEnd.Value.ToString("MM/dd/yyyy") : "TBA")}"
                                 },

                    _ => from batch in await _repository.RetrieveAllAsync()
                         join course in await _courseRepository.RetrieveAllAsync() on batch.CourseId equals course.Id
                         join trainor in await _userRepository.GetUsersAsync() on batch.TrainorId equals trainor.Id
                         orderby batch.RQMNumber, course.ProgramTitle, trainor.FirstName
                         select new BatchListViewModel
                         {
                             Id = batch.Id,
                             RQMCode = batch.RQMNumber,
                             ProgramTitle = course.ProgramTitle,
                             TrainorName = $"{trainor.FirstName} {trainor.MiddleName![0]} {trainor.LastName} {trainor.ExtensionName}",
                             Period = $"{batch.DateStart:MM/dd/yyyy} - {(batch.DateEnd.HasValue ? batch.DateEnd.Value.ToString("MM/dd/yyyy") : "TBA")}"
                         }
            };
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime} SearchBatchListAsync Failed: {message}", DateTime.Now.ToString(), ex.Message);
                throw;
            }
        }

        public async Task<IQueryable<BatchCourseListViewModel>> SearchBatchCourseListAsync(string courseId, string searchType, string searchInput)
        {
            try
            {
                return searchType switch
                {
                    "RQM" => from batch in await _repository.GetBatchesFromCourse(courseId)
                             join trainor in await _userRepository.GetUsersAsync() on batch.TrainorId equals trainor.Id
                             orderby batch.RQMNumber, trainor.FirstName
                             where batch.RQMNumber.Contains(searchInput)
                             select new BatchCourseListViewModel
                             {
                                 Id = batch.Id,
                                 RQMCode = batch.RQMNumber,
                                 Trainor = $"{trainor.FirstName} {trainor.MiddleName![0]} {trainor.LastName} {trainor.ExtensionName}",
                                 DateStarted = batch.DateStart.ToShortDateString(),
                                 DateFinished = batch.DateEnd.HasValue ? batch.DateEnd.Value.ToShortDateString() : "TBA",
                                 LearningMode = GetEnumDescription(Enum.Parse<LearningMode>(batch.LearningMode))
                             },

                    "Trainor" => from batch in await _repository.GetBatchesFromCourse(courseId)
                                 join trainor in await _userRepository.GetUsersAsync() on batch.TrainorId equals trainor.Id
                                 orderby batch.RQMNumber, trainor.FirstName
                                 where
                                    trainor.FirstName!.Contains(searchInput) ||
                                    trainor.MiddleName!.Contains(searchInput) ||
                                    trainor.LastName!.Contains(searchInput) ||
                                    trainor.ExtensionName!.Contains(searchInput)
                                 select new BatchCourseListViewModel
                                 {
                                     Id = batch.Id,
                                     RQMCode = batch.RQMNumber,
                                     Trainor = $"{trainor.FirstName} {trainor.MiddleName![0]} {trainor.LastName} {trainor.ExtensionName}",
                                     DateStarted = batch.DateStart.ToShortDateString(),
                                     DateFinished = batch.DateEnd.HasValue ? batch.DateEnd.Value.ToShortDateString() : "TBA",
                                     LearningMode = GetEnumDescription(Enum.Parse<LearningMode>(batch.LearningMode))
                                 },

                    _ => from batch in await _repository.GetBatchesFromCourse(courseId)
                         join trainor in await _userRepository.GetUsersAsync() on batch.TrainorId equals trainor.Id
                         orderby batch.RQMNumber, trainor.FirstName
                         select new BatchCourseListViewModel
                         {
                             Id = batch.Id,
                             RQMCode = batch.RQMNumber,
                             Trainor = $"{trainor.FirstName} {trainor.MiddleName![0]} {trainor.LastName} {trainor.ExtensionName}",
                             DateStarted = batch.DateStart.ToShortDateString(),
                             DateFinished = batch.DateEnd.HasValue ? batch.DateEnd.Value.ToShortDateString() : "TBA",
                             LearningMode = GetEnumDescription(Enum.Parse<LearningMode>(batch.LearningMode))
                         }
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CreateBatch> GetCreateBatchDTOAsync(string? courseId)
        {
            try
            {
                var TrainorList = (from trainor in await _userRepository.GetUsersAsync()
                                join urole in await _userRepository.GetUserRolesAsync() on trainor.Id equals urole.UserId
                                join role in await _userRepository.GetRolesAsync() on urole.RoleId equals role.Id
                                orderby trainor.FirstName
                                where
                                     role.Name!.Contains("Trainor")
                                select new TrainorList
                                {
                                    TrainorId = trainor.Id,
                                    Name = $"{trainor.FirstName} {trainor.MiddleName![0]} {trainor.LastName} {trainor.ExtensionName}"
                                });
                var CourseList = from course in await _courseRepository.RetrieveAllAsync()
                                 orderby course.ProgramTitle
                                 select new CourseList
                                 {
                                     CourseId = course.Id,
                                     Name = course.ProgramTitle,
                                     Duration = course.Duration
                                 };
                return new CreateBatch
                {
                    CourseId = courseId ?? null,
                    CourseList = [.. CourseList],
                    TrainorList = [.. TrainorList],
                    FromCoursePage = !courseId.IsNullOrEmpty()
                };

            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<UpdateBatch> GetUpdateBatchDTOAsync(string Id, bool FromCoursePage = false)
        {
            try
            {
                var TrainorList = (from trainor in await _userRepository.GetUsersAsync()
                                   join urole in await _userRepository.GetUserRolesAsync() on trainor.Id equals urole.UserId
                                   join role in await _userRepository.GetRolesAsync() on urole.RoleId equals role.Id
                                   orderby trainor.FirstName
                                   where
                                        role.Name!.Contains("Trainor")
                                   select new TrainorList
                                   {
                                       TrainorId = trainor.Id,
                                       Name = $"{trainor.FirstName} {trainor.MiddleName![0]} {trainor.LastName} {trainor.ExtensionName}"
                                   });
                var CourseList = from course in await _courseRepository.RetrieveAllAsync()
                                 orderby course.ProgramTitle
                                 select new CourseList
                                 {
                                     CourseId = course.Id,
                                     Name = course.ProgramTitle,
                                     Duration = course.Duration
                                 };
                var batch = await _repository.GetBatchAsync(Id) ?? throw new NullReferenceException("Batch not Found");
                if (batch.DeploymentDetails == null) throw new NullReferenceException("Batch Deployment Detail not Found");
                return new UpdateBatch
                {
                    Id = Id,
                    CourseId = batch.CourseId,
                    CourseList = [.. CourseList],
                    TrainorId = batch.TrainorId,
                    TrainorList = [.. TrainorList],
                    LearningDelivery = Enum.Parse<LearningDelivery>(batch.LearningDelivery),
                    LearningMode = Enum.Parse<LearningMode>(batch.LearningMode),
                    RQMNumber = batch.RQMNumber,
                    DateStart = batch.DateStart,
                    DateEnd = batch.DateEnd,
                    TimeStart = batch.TimeStart,
                    TimeEnd = batch.TimeEnd,
                    EmployerName = batch.DeploymentDetails.EmployerName,
                    EmployerAddress = batch.DeploymentDetails.EmployerAddress,
                    Occupation = batch.DeploymentDetails.Occupation,
                    Classification = batch.DeploymentDetails.Classification,
                    Salary = batch.DeploymentDetails.Salary,
                    FromCoursePage = FromCoursePage,
                    NTPId = batch.NTPId,
                    CertificatesId = batch.CertificatesId,
                    CoursePageId = FromCoursePage? batch.CourseId : null,
                    OverwriteNTP = false,
                    OverwriteCertificate = false,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BatchDetailViewModel> GetBatchDetailAsync(string Id, bool FromCoursePage = false)
        {
            try
            {
                var batch = await _repository.GetBatchAsync(Id) ?? throw new NullReferenceException("Batch not found");
                batch.Trainor = await _userRepository.GetUserByIdAsync(batch.TrainorId) ?? throw new NullReferenceException("Trainor not found");
                batch.Course = await _courseRepository.GetCourseAsync(batch.CourseId) ?? throw new NullReferenceException("Course not found");

                var trainor = $"{batch.Trainor.FirstName} {batch.Trainor.MiddleName![0]} {batch.Trainor.LastName} {batch.Trainor.ExtensionName}";
                var TimeStart = batch.TimeStart.HasValue ? batch.TimeStart.Value.ToString("hh:mm tt") : "TBA";
                var TimeEnd = batch.TimeEnd.HasValue ? batch.TimeEnd.Value.ToString("hh:mm tt") : "TBA";
                var DateStart = batch.DateStart.ToString("MM/dd/yyyy");
                var DateEnd = batch.DateEnd.HasValue? batch.DateEnd.Value.ToString("MM/dd/yyyy") : "TBA";

                BatchDetailViewModel detail = new()
                {
                    Id = batch.Id,
                    ProgramTitle = batch.Course.ProgramTitle,
                    RQMCode = batch.RQMNumber,
                    TrainorName = trainor,
                    Session = $"{TimeStart} - {TimeEnd}",
                    LearningMode = GetEnumDescription(Enum.Parse<LearningMode>(batch.LearningMode)),
                    LearningDelivery = GetEnumDescription(Enum.Parse<LearningDelivery>(batch.LearningDelivery)),
                    Schedule = $"{DateStart} - {DateEnd}",
                    NTPId = batch.NTPId,
                    CertificatesId = batch.CertificatesId ?? null,
                    CanBeDeleted = false,
                };
                if(FromCoursePage) detail.CourseId = batch.CourseId;
                //Check if can be deleted
                detail.CanBeDeleted = true;
                return detail;
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime} GetBatchListAsync Failed: {message}", DateTime.Now.ToString(), ex.Message);
                throw;
            }
        }

        public async Task<Work> AddBatchAsync(CreateBatch model)
        {
            try
            {
                //Check for unique RQM
                if(model.RQMNumber == null) throw new NullReferenceException("Null RQMNumber");
                string RQMCode = model.RQMNumber.ToUpperInvariant();
                var checkifExist = await _repository.GetBatchAsync(RQMCode);
                if (checkifExist != null) throw new ArgumentException($"Batch with RQM Code \'{RQMCode}\' already Exist");

                //Batch and its DeploymentDetails and attached PDFs ID should be the RQM
                string Id = RQMCode;

                //Create NTP PDF
                string NTPName = $"{Id}_NTP.pdf";

                Batch batch = new()
                {
                    Id = Id,
                    CourseId = model.CourseId!,
                    Course = await _courseRepository.GetCourseAsync(model.CourseId!) ?? throw new NullReferenceException("Course not Found"),
                    TrainorId = model.TrainorId!,
                    Trainor = await _userRepository.GetUserByIdAsync(model.TrainorId!) ?? throw new NullReferenceException("Trainor not Found"),
                    LearningDelivery = model.LearningDelivery.ToString() ?? throw new NullReferenceException("LearningDelivery not Found"),
                    LearningMode = model.LearningMode.ToString() ?? throw new NullReferenceException("LearningMode not Found"),
                    RQMNumber = RQMCode,
                    DateStart = model.DateStart!.Value,
                    DateEnd = model.DateEnd.HasValue ? model.DateEnd.Value : null,
                    TimeStart = model.TimeStart,
                    TimeEnd = model.TimeEnd,
                    NTPId = NTPName,
                    NTP = await _pdfRepository.CreatePDF(NTPName, NTPName, model.NTP!)
                };
                DeploymentDetails deploymentDetails = new()
                {
                    Id = Id,
                    BatchId = Id,
                    Batch = batch,
                    EmployerName = model.EmployerName,
                    EmployerAddress = model.EmployerAddress,
                    Occupation = model.Occupation,
                    Classification = model.Classification,
                    Salary = model.Salary
                };
                batch.DeploymentDetails = deploymentDetails;

                //Add Certificates if exists
                if (model.Certificates != null)
                {
                    var CertificateName = $"{Id}_Certificates.pdf";
                    batch.CertificatesId = CertificateName;
                    batch.Certificates = await _pdfRepository.CreatePDF(CertificateName, CertificateName, model.Certificates!);
                }

                var work = await _repository.CreateBatchAsync(batch, deploymentDetails);
                if (!work) throw new Exception("Work failed");

                _work.Time = DateTime.Now;
                _work.Message = "Successfully Created Batch";
                _work.Result = true;
                return _work;
            }
            catch (ArgumentException ex)
            {
                _work.ErrorCode = ErrorType.Argument;
                _work.Time = DateTime.Now;
                _work.Message = ex.Message;
                _work.Result = false;
                return _work;
            }
            catch (Exception ex)
            {
                _work.ErrorCode = ex.Message;
                _work.Time = DateTime.Now;
                _work.Message = "Couldn't Create Batch";
                _work.Result = false;
                return _work;
            }
        }

        public async Task<Work> UpdateBatchAsync(UpdateBatch model)
        {
            try
            {
                var batch = await _repository.GetBatchAsync(model.Id) ?? throw new NullReferenceException("Batch not Found");
                batch.CourseId = model.CourseId!;
                batch.TrainorId = model.TrainorId!;
                batch.LearningDelivery = model.LearningDelivery.ToString() ?? throw new NullReferenceException("LearningDelivery not Found");
                batch.LearningMode = model.LearningMode.ToString() ?? throw new NullReferenceException("LearningMode not Found");
                batch.RQMNumber = model.RQMNumber!;
                batch.DateStart = model.DateStart!.Value;
                batch.DateEnd = model.DateEnd;
                batch.TimeStart = model.TimeStart;
                batch.TimeEnd = model.TimeEnd;

                if (batch.DeploymentDetails == null) throw new NullReferenceException("Batch Deployment Details not Found");
                batch.DeploymentDetails.EmployerName = model.EmployerName;
                batch.DeploymentDetails.EmployerAddress = model.EmployerAddress;
                batch.DeploymentDetails.Occupation = model.Occupation;
                batch.DeploymentDetails.Classification = model.Classification;
                batch.DeploymentDetails.Salary = model.Salary;

                if(model.OverwriteNTP)
                {
                    if (model.NTP != null)
                        _ = await _pdfRepository.UpdatePDFFile(batch.NTPId, model.NTP);
                    else
                        throw new NullReferenceException("NTP PDF not found");
                }

                // Upload Certificate if batch does not have one yet
                if (batch.CertificatesId == null && model.Certificates != null) 
                {
                    var CertificateName = $"{batch.Id}_Certificates.pdf";
                    batch.CertificatesId = CertificateName;
                    batch.Certificates = await _pdfRepository.CreatePDF(CertificateName, CertificateName, model.Certificates);
                }
                else
                {
                    if (model.OverwriteCertificate) //Check if to delete or update Certificates
                    {
                        if (batch.CertificatesId != null)
                        {
                            // Delete Certificate
                            if (model.Certificates == null) 
                            {
                                _ = await _pdfRepository.DeletePDF(batch.CertificatesId);
                                batch.CertificatesId = null;
                            }
                            // Update Certificate
                            else
                                _ = await _pdfRepository.UpdatePDFFile(batch.CertificatesId, model.Certificates);
                        }
                    }
                }

                var work = await _repository.UpdateBatchAsync(batch, batch.DeploymentDetails);
                if (!work) throw new Exception("Work failed");

                _work.Time = DateTime.Now;
                _work.Message = "Successfully Updated Batch";
                _work.Result = true;
                return _work;
            }
            catch (Exception ex)
            {
                _work.ErrorCode = ex.Message;
                _work.Time = DateTime.Now;
                _work.Message = "Couldn't Update Batch";
                _work.Result = false;
                return _work;
            }
        }

        public async Task<Work> DeleteBatchAsync(string Id)
        {
            try
            {
                var batch = await _repository.GetBatchAsync(Id) ?? throw new NullReferenceException("Batch not found");
                //Check if it still has students


                var work = await _repository.DeleteBatchAsync(batch.Id);
                if (!work) throw new Exception("Work failed");

                //Delete attached document
                _ = await _pdfRepository.DeletePDF(batch.NTPId);
                if (batch.CertificatesId != null) _ = await _pdfRepository.DeletePDF(batch.CertificatesId);

                _work.Time = DateTime.Now;
                _work.Message = "Successfully Deleted Batch";
                _work.Result = true;
                return _work;
            }
            catch (Exception ex)
            {
                _work.ErrorCode = ex.Message;
                _work.Time = DateTime.Now;
                _work.Message = "Couldn't Delete Batch";
                _work.Result = false;
                return _work;
            }
        }

        public async Task<FileContentResult> GetDocument(string Id)
        {
            try
            {
                return await _pdfRepository.GetPDFFile(Id);
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
