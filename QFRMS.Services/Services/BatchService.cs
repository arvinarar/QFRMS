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
using QFRMS.Data.Repositories;
using QFRMS.Data.Enums;
using System.Globalization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Net;
using System.Net.NetworkInformation;

namespace QFRMS.Services.Services
{
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository _repository;
        private readonly IAboutRepository _aboutRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserAccountRepository _userRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IPDFRepository _pdfRepository;
        private readonly ILogger<BatchService> _logger;
        private readonly Work _work = new Work();

        public BatchService(IBatchRepository repository, IAboutRepository aboutRepository, ICourseRepository courseRepository, IUserAccountRepository userAccountRepository, IStudentRepository studentRepository, IPDFRepository pdfRepository, ILogger<BatchService> logger)
        {
            _repository = repository;
            _aboutRepository = aboutRepository;
            _courseRepository = courseRepository;
            _userRepository = userAccountRepository;
            _studentRepository = studentRepository;
            _pdfRepository = pdfRepository;
            _logger = logger;
        }

        public async Task<IQueryable<BatchListViewModel>> GetBatchListAsync(string? TrainorName)
        {
            try
            {
                if(TrainorName.IsNullOrEmpty())
                {
                    return await Task.FromResult(from batch in await _repository.RetrieveAllAsync()
                                                 join course in await _courseRepository.RetrieveAllAsync() on batch.CourseId equals course.Id
                                                 join trainor in await _userRepository.GetUsersAsync() on batch.TrainorId equals trainor.Id
                                                 orderby course.ProgramTitle, batch.RQMNumber, trainor.FirstName
                                                 select new BatchListViewModel
                                                 {
                                                     Id = batch.Id,
                                                     RQMCode = batch.RQMNumber,
                                                     ProgramTitle = course.ProgramTitle,
                                                     TrainorName = $"{trainor.FirstName} {trainor.MiddleName![0]} {trainor.LastName} {trainor.ExtensionName}",
                                                     Period = $"{batch.DateStart:MM/dd/yyyy} - {(batch.DateEnd.HasValue ? batch.DateEnd.Value.ToString("MM/dd/yyyy") : "TBA")}"
                                                 });
                }
                else
                {
                    return await Task.FromResult(from batch in await _repository.RetrieveAllAsync()
                                                 join course in await _courseRepository.RetrieveAllAsync() on batch.CourseId equals course.Id
                                                 join trainor in await _userRepository.GetUsersAsync() on batch.TrainorId equals trainor.Id
                                                 where trainor.UserName == TrainorName
                                                 orderby course.ProgramTitle, batch.RQMNumber, trainor.FirstName
                                                 select new BatchListViewModel
                                                 {
                                                     Id = batch.Id,
                                                     RQMCode = batch.RQMNumber,
                                                     ProgramTitle = course.ProgramTitle,
                                                     TrainorName = $"{trainor.FirstName} {trainor.MiddleName![0]} {trainor.LastName} {trainor.ExtensionName}",
                                                     Period = $"{batch.DateStart:MM/dd/yyyy} - {(batch.DateEnd.HasValue ? batch.DateEnd.Value.ToString("MM/dd/yyyy") : "TBA")}"
                                                 });
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IQueryable<BatchListViewModel>> SearchBatchListAsync(string searchType, string searchInput, string? TrainorName)
        {
            try
            {
                if (TrainorName.IsNullOrEmpty())
                {
                    return searchType switch
                    {
                        "RQM" => from batch in await _repository.RetrieveAllAsync()
                                 join course in await _courseRepository.RetrieveAllAsync() on batch.CourseId equals course.Id
                                 join trainor in await _userRepository.GetUsersAsync() on batch.TrainorId equals trainor.Id
                                 orderby course.ProgramTitle, batch.RQMNumber, trainor.FirstName
                                 where
                                    batch.RQMNumber.Contains(searchInput)
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
                                   orderby course.ProgramTitle, batch.RQMNumber, trainor.FirstName
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
                                     orderby course.ProgramTitle, batch.RQMNumber, trainor.FirstName
                                     where
                                        trainor.FirstName!.Contains(searchInput) ||
                                        trainor.MiddleName!.Contains(searchInput) ||
                                        trainor.LastName!.Contains(searchInput) ||
                                        trainor.ExtensionName == searchInput
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
                             orderby course.ProgramTitle, batch.RQMNumber, trainor.FirstName
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
                else
                {
                    return searchType switch
                    {
                        "RQM" => from batch in await _repository.RetrieveAllAsync()
                                 join course in await _courseRepository.RetrieveAllAsync() on batch.CourseId equals course.Id
                                 join trainor in await _userRepository.GetUsersAsync() on batch.TrainorId equals trainor.Id
                                 orderby course.ProgramTitle, batch.RQMNumber, trainor.FirstName
                                 where
                                    trainor.UserName == TrainorName &&
                                    batch.RQMNumber.Contains(searchInput)
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
                                   orderby course.ProgramTitle, batch.RQMNumber, trainor.FirstName
                                   where 
                                    trainor.UserName == TrainorName &&
                                    course.ProgramTitle.Contains(searchInput)
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
                             where trainor.UserName == TrainorName
                             orderby course.ProgramTitle, batch.RQMNumber, trainor.FirstName
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
            }
            catch (Exception)
            {
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
                                    trainor.ExtensionName == searchInput
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

                //Deployment Details, will not show if all fiels are empty
                var EN = batch.DeploymentDetails!.EmployerName;
                var EA = batch.DeploymentDetails!.EmployerAddress;
                var Oc = batch.DeploymentDetails.Occupation;
                var Cl = batch.DeploymentDetails.Classification;
                var Sa = batch.DeploymentDetails.Salary;
                bool HasDeploymentDetails = !string.IsNullOrEmpty(EN + EA + Oc + Cl + Sa);

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
                    EmployerName = EN,
                    EmployerAddress = EA,
                    Occupation = Oc,
                    Classification = Cl,
                    Salary = Sa,
                    HasDeploymentDetail = HasDeploymentDetails
                };
                if(FromCoursePage) detail.CourseId = batch.CourseId;
                //Check if can be deleted
                if (_studentRepository.RetrieveStudentsFromBatchAsync(Id).Result.Any())
                    detail.CanBeDeleted = false;
                else
                    detail.CanBeDeleted = true;

                return detail;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Work> AddBatchAsync(CreateBatch model)
        {
            try
            {
                //Check for unique RQM
                if(model.RQMNumber == null) throw new ArgumentException("RQM Code is required");
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
            catch (NullReferenceException ex)
            {
                _work.ErrorCode = ex.Message;
                _work.Time = DateTime.Now;
                _work.Message = "Couldn't Create Batch";
                _work.Result = false;
                return _work;
            }
            catch (Exception)
            {
                throw;
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

                _work.Time = DateTime.Now;
                _work.Message = "Successfully Updated Batch";
                _work.Result = true;
                return _work;
            }
            catch (NullReferenceException ex)
            {
                _work.ErrorCode = ex.Message;
                _work.Time = DateTime.Now;
                _work.Message = "Couldn't Update Batch";
                _work.Result = false;
                return _work;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Work> DeleteBatchAsync(string Id)
        {
            try
            {
                var batch = await _repository.GetBatchAsync(Id) ?? throw new NullReferenceException("Batch not found");
                //Check if it still has students


                var work = await _repository.DeleteBatchAsync(batch.Id);

                //Delete attached document
                _ = await _pdfRepository.DeletePDF(batch.NTPId);
                if (batch.CertificatesId != null) _ = await _pdfRepository.DeletePDF(batch.CertificatesId);

                _work.Time = DateTime.Now;
                _work.Message = "Successfully Deleted Batch";
                _work.Result = true;
                return _work;
            }
            catch (NullReferenceException ex)
            {
                _work.ErrorCode = ex.Message;
                _work.Time = DateTime.Now;
                _work.Message = "Couldn't Delete Batch";
                _work.Result = false;
                return _work;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Work> AddStudentsFromCSV(ImportSheet model)
        {
            try
            {
                if (model.File == null) throw new NullReferenceException("NO CSV file found");
                using (var filestream = model.File.OpenReadStream())
                {
                    var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(filestream);
                    parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                    parser.SetDelimiters([","]);

                    //Get CSV Header if CSV is valid and comes from the T2MIS website.
                    //by checking the the first and last field of header line
                    string[] headers = parser.ReadFields() ?? throw new ArgumentException("Invalid CSV File");
                    if (!headers[0].Contains("Region") && !headers[47].Contains("Salary")) throw new ArgumentException("Invalid CSV File");

                    List<Student> NewStudents = new List<Student>(); //For Adding New Students
                    List<Student> ExistingStudents = new List<Student>(); //For Updating Existing Students

                    //Get Batch
                    var batch = await _repository.GetBatchAsync(model.BatchId) ?? throw new NullReferenceException("No Batch with RQM Code Found");

                    //Set indexes as constants
                    const int LastName = 15;
                    const int FirstName = 16;
                    const int MiddleName = 17;
                    const int ExtensionName = 18;
                    const int ULI = 19;
                    const int ContactNo = 20;
                    const int Email = 21;
                    const int StreetNo = 22;
                    const int Barangay = 23;
                    const int MunicipalityCity = 24;
                    const int District = 25;
                    const int Province = 26;
                    const int Sex = 27;
                    const int BirthDate = 28;
                    const int Age = 29;
                    const int CivilStatus = 30;
                    const int HighestGrade = 31;
                    const int Nationality = 32;
                    const int TS = 34;
                    const int ESBT = 41;

                    string ULIDuplicateChecker = "";
                    while (!parser.EndOfData)
                    {
                        string[]? row = parser.ReadFields();
                        if (row == null || row.Length == 0) continue;

                        //Check if row is duplicating, T2MIS bogaloo
                        if (row[ULI].Equals(ULIDuplicateChecker)) continue;
                        ULIDuplicateChecker = row[ULI];

                        //Check if Student already exist
                        Console.WriteLine(row[ULI]);
                        Student? student = await _studentRepository.GetStudentAsync(row[ULI]);

                        //New Student
                        if (student == null)
                        {
                            student = new Student
                            {
                                ULI = row[ULI],
                                BatchId = model.BatchId,
                                Batch = batch,
                                FirstName = row[FirstName],
                                MiddleName = row[MiddleName],
                                LastName = row[LastName],
                                ExtensionName = row[ExtensionName],
                                ContactNo = row[ContactNo],
                                Email = row[Email],
                                StreetNo = row[StreetNo],
                                Barangay = row[Barangay],
                                MunicipalityCity = row[MunicipalityCity],
                                District = row[District],
                                Province = row[Province],
                                Sex = GetValueFromDescription<Sex>(row[Sex]),
                                BirthDate = DateTime.ParseExact(row[BirthDate], "MM/dd/yyyy", CultureInfo.InvariantCulture),
                                Age = int.Parse(row[Age]),
                                CivilStatus = GetValueFromDescription<CivilStatus>(row[CivilStatus]),
                                HighestGrade = GetValueFromDescription<HighestGrade>(row[HighestGrade]),
                                Nationality = row[Nationality],
                                TrainingStatus = row[TS].IsNullOrEmpty() ? TrainingStatus.Ongoing : GetValueFromDescription<TrainingStatus>(row[TS]),
                                ESBT = GetValueFromDescription<ESBT>(row[ESBT])
                            };
                            NewStudents.Add(student);
                        }
                        else
                        {
                            student.BatchId = model.BatchId;
                            student.Batch = batch;
                            student.FirstName = row[FirstName];
                            student.MiddleName = row[MiddleName];
                            student.LastName = row[LastName];
                            student.ExtensionName = row[ExtensionName];
                            student.ContactNo = row[ContactNo];
                            student.Email = row[Email];
                            student.StreetNo = row[StreetNo];
                            student.Barangay = row[Barangay];
                            student.MunicipalityCity = row[MunicipalityCity];
                            student.District = row[District];
                            student.Province = row[Province];
                            student.Sex = GetValueFromDescription<Sex>(row[Sex]);
                            student.BirthDate = DateTime.ParseExact(row[BirthDate], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                            student.Age = int.Parse(row[Age]);
                            student.CivilStatus = GetValueFromDescription<CivilStatus>(row[CivilStatus]);
                            student.HighestGrade = GetValueFromDescription<HighestGrade>(row[HighestGrade]);
                            student.Nationality = row[Nationality];
                            student.TrainingStatus = row[TS].IsNullOrEmpty() ? TrainingStatus.Ongoing : GetValueFromDescription<TrainingStatus>(row[TS]);
                            student.ESBT = GetValueFromDescription<ESBT>(row[ESBT]);
                            ExistingStudents.Add(student);
                        }
                    }
                    if(NewStudents.Count > 0)
                    {
                        var AS = await _studentRepository.AddStudents(NewStudents);
                    }
                    if(ExistingStudents.Count > 0)
                    {
                        var US = await _studentRepository.UpdateStudents(NewStudents);
                    }
                }
                _work.Time = DateTime.Now;
                _work.Message = $"Successfully Added/Updated Students of Batch {model.BatchId}";
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
            catch (NullReferenceException ex)
            {
                _work.ErrorCode = ex.Message;
                _work.Time = DateTime.Now;
                _work.Message = "Couldn't Add Student from CSV";
                _work.Result = false;
                return _work;
            }
            catch (Exception)
            {
                throw;
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

        public async Task<bool> CheckIfAlreadyExist(string RQM)
        {
            try
            {
                return await _repository.GetBatchAsync(RQM) != null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FileContentResult> GenerateTerminalReport(string Id, string? registrarUsername)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    //Get all neccessary models for the Terminal Report
                    var InstituteInfo = await _aboutRepository.GetInstituteInfosAsync() ?? throw new NullReferenceException("Institute Info not found");
                    var batch = await _repository.GetBatchAsync(Id) ?? throw new NullReferenceException("Batch with RQM Code not found");
                    var deploymentDetails = batch.DeploymentDetails ?? throw new NullReferenceException("Batch Deployment Details not found");
                    var course = await _courseRepository.GetCourseOfBatchAsync(batch.CourseId) ?? throw new NullReferenceException("Course not found");
                    var students = _studentRepository.RetrieveStudentsFromBatchAsync(Id).Result.ToList() ?? throw new NullReferenceException("Students not found");
                    var registrar = await _userRepository.GetUserByName(registrarUsername ?? throw new NullReferenceException("Registrar not found")) ?? throw new NullReferenceException("Registrar not found");
                    var trainor = await _userRepository.GetUserByIdAsync(batch.TrainorId) ?? throw new NullReferenceException("Trainor not found"); ;
                    var registrarName = $"{registrar.FirstName} {registrar.MiddleName![0]}. {registrar.LastName} {registrar.ExtensionName}";
                    var trainorName = $"{trainor.FirstName} {trainor.MiddleName![0]}. {trainor.LastName} {trainor.ExtensionName}";

                    //Create WorkSheet
                    var worksheet = package.Workbook.Worksheets.Add("Terminal Report");
                    worksheet.View.PageLayoutView = true;

                    //Set Worksheet size and margin
                    worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                    worksheet.PrinterSettings.PaperSize = ePaperSize.A4;
                    worksheet.PrinterSettings.TopMargin = 0.75m;
                    worksheet.PrinterSettings.BottomMargin = 0.75m;
                    worksheet.PrinterSettings.LeftMargin = 0.25m;
                    worksheet.PrinterSettings.RightMargin = 0.25m;
                    worksheet.PrinterSettings.HeaderMargin = 0.3m;
                    worksheet.PrinterSettings.FooterMargin = 0.3m;

                    //Set Worksheet header
                    worksheet.HeaderFooter.ScaleWithDocument = true;
                    worksheet.HeaderFooter.AlignWithMargins = false;
                    worksheet.HeaderFooter.OddHeader.LeftAlignedText = $"T2MIS TERMINAL REPORT {batch.DateStart:MM/dd/yy} - {batch.DateEnd:MM/dd/yy}";
                    worksheet.HeaderFooter.OddFooter.LeftAlignedText = $"&U&\"-,Bold\"{registrarName.ToUpper()}&\"-,Regular\"&U\r\nRegistrar";
                    worksheet.HeaderFooter.OddFooter.CenteredText = $"&U&\"-,Bold\"{trainorName.ToUpper()}&\"-,Regular\"&U\r\nTrainor";
                    worksheet.HeaderFooter.OddFooter.RightAlignedText = $"&U&\"-,Bold\"{InstituteInfo.FocalPerson.ToUpper()}&\"-,Regular\"&U\r\nFarm School Administrator";

                    //Format Cells
                    worksheet.Cells["A1:AV26"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A1:AV26"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A1:AV26"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A1:AV26"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A1:AV26"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells["A1:AV26"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells["A1:AV26"].Style.Font.Name = "Segoe UI";
                    worksheet.Cells["A1:AV26"].Style.Font.Size = 10;
                    worksheet.Cells["A1:AV26"].Style.WrapText = true;

                    //Set Row Heights and Column Widths
                    worksheet.Cells["A1:AV26"].EntireRow.Height = 15;
                    //Page 1
                    worksheet.Column(1).Width = TrueColumnWidth(25); // Region
                    worksheet.Column(2).Width = TrueColumnWidth(5); // Province
                    worksheet.Column(3).Width = TrueColumnWidth(3); // Congressional District
                    worksheet.Column(4).Width = TrueColumnWidth(11); // Municipality City
                    worksheet.Column(5).Width = TrueColumnWidth(55); // Name of Provider
                    worksheet.Column(6).Width = TrueColumnWidth(25); // Complete Address
                    worksheet.Column(7).Width = TrueColumnWidth(7); // Type of Provider
                    worksheet.Column(8).Width = TrueColumnWidth(5); // Classification of Provider
                    //Page 2
                    worksheet.Column(9).Width = TrueColumnWidth(30); // Industry, Sector of Qualification
                    worksheet.Column(10).Width = TrueColumnWidth(4); // TVET Program Registration Status
                    worksheet.Column(11).Width = TrueColumnWidth(59); // Qualification Program Title
                    worksheet.Column(12).Width = TrueColumnWidth(2); // Cluster
                    worksheet.Column(13).Width = TrueColumnWidth(13); // CTPR
                    worksheet.Column(14).Width = TrueColumnWidth(2); // Training Calendar Code
                    worksheet.Column(15).Width = TrueColumnWidth(27); // Delivery Mode
                    //Page 3
                    worksheet.Column(16).Width = TrueColumnWidth(14); // Last Name
                    worksheet.Column(17).Width = TrueColumnWidth(20); // First Name
                    worksheet.Column(18).Width = TrueColumnWidth(14); // Middle Name
                    worksheet.Column(19).Width = TrueColumnWidth(3); // Extension Name
                    worksheet.Column(20).Width = TrueColumnWidth(20); // ULI
                    worksheet.Column(21).Width = TrueColumnWidth(13); // Contact Number
                    worksheet.Column(22).Width = TrueColumnWidth(26); // E mail Address
                    worksheet.Column(23).Width = TrueColumnWidth(26); // Street No and Street address
                    //Page 4
                    worksheet.Column(24).Width = TrueColumnWidth(16); // Barangay
                    worksheet.Column(25).Width = TrueColumnWidth(14); // Municipality City
                    worksheet.Column(26).Width = TrueColumnWidth(3); // District
                    worksheet.Column(27).Width = TrueColumnWidth(5); // Province 
                    worksheet.Column(28).Width = TrueColumnWidth(7); // Sex
                    worksheet.Column(29).Width = TrueColumnWidth(10); // Date of Birth
                    worksheet.Column(30).Width = TrueColumnWidth(3); // Age
                    worksheet.Column(31).Width = TrueColumnWidth(9); // Civil Status
                    worksheet.Column(32).Width = TrueColumnWidth(24); // Highest Grade Completed
                    worksheet.Column(33).Width = TrueColumnWidth(8); // Nationality
                    worksheet.Column(34).Width = TrueColumnWidth(10); // Classification of Clients
                    worksheet.Column(35).Width = TrueColumnWidth(9); // Training Status
                    worksheet.Column(36).Width = TrueColumnWidth(15); // Type of Scholarships
                    //Page 5
                    worksheet.Column(37).Width = TrueColumnWidth(25); // Voucher Number
                    worksheet.Column(38).Width = TrueColumnWidth(10); // Date Started
                    worksheet.Column(39).Width = TrueColumnWidth(10); // Date Finished
                    worksheet.Column(40).Width = TrueColumnWidth(2); // Date Assessed
                    worksheet.Column(41).Width = TrueColumnWidth(2); // Assessment Results
                    worksheet.Column(42).Width = TrueColumnWidth(13); // Employment Status Before the Training
                    worksheet.Column(43).Width = TrueColumnWidth(10); // Date Of Employment
                    worksheet.Column(44).Width = TrueColumnWidth(7); // Occupation
                    worksheet.Column(45).Width = TrueColumnWidth(7); // Name of Employer
                    worksheet.Column(46).Width = TrueColumnWidth(24); // Address
                    worksheet.Column(47).Width = TrueColumnWidth(12); // Classification
                    worksheet.Column(48).Width = TrueColumnWidth(12); // Salary

                    //Prepare the sheet Fields
                    List<TerminalReport> TerminalReportList = [];
                    foreach(var student in students)
                    {
                        var terminalReport = new TerminalReport
                        {
                            I_Region = InstituteInfo.Region,
                            I_Province = InstituteInfo.Province,
                            I_District = InstituteInfo.District,
                            I_City = InstituteInfo.City,
                            I_Name = InstituteInfo.Name,
                            I_Address = InstituteInfo.Address,
                            I_ProviderType = InstituteInfo.ProviderType,
                            I_ProviderClassification = InstituteInfo.ProviderClassification,
                            C_Sector = course.Sector,
                            C_Status = course.Status,
                            C_ProgramTitle = course.ProgramTitle,
                            C_COPRNo = course.COPRNo,
                            C_DeliveryMode = course.DeliveryMode,
                            S_LastName = student.LastName,
                            S_FirstName = student.FirstName,
                            S_MiddleName = student.MiddleName,
                            S_ExtensionName = student.ExtensionName,
                            S_ULI = student.ULI,
                            S_ContactNo = Int64.TryParse(student.ContactNo, out Int64 contactNo) ? $"0{contactNo:###-###-####}" : student.ContactNo,
                            S_Email = student.Email,
                            S_StreetNo = student.StreetNo,
                            S_Barangay = student.Barangay,
                            S_MunicipalityCity = student.MunicipalityCity,
                            S_District = student.District,
                            S_Province = student.Province,
                            S_Sex = GetEnumDescription(student.Sex),
                            S_BirthDate = student.BirthDate.ToString("MM/dd/yyyy"),
                            S_Age = student.Age.ToString(),
                            S_CivilStatus = GetEnumDescription(student.CivilStatus),
                            S_HighestGrade = GetEnumDescription(student.HighestGrade),
                            S_Nationality = student.Nationality.ToUpper(),
                            C_ClientClassification = course.ClientClassification,
                            S_TrainingStatus = (student.TrainingStatus == TrainingStatus.Ongoing) ? "": GetEnumDescription(student.TrainingStatus),
                            C_ScholarshipType = course.ScholarshipType,
                            B_VoucherNo = batch.RQMNumber,
                            B_DateStart = batch.DateStart.ToString("MM/dd/yyyy"),
                            B_DateEnd = batch.DateEnd.HasValue ? batch.DateEnd.Value.ToString("MM/dd/yyyy") : "",
                            S_ESBT = GetEnumDescription(student.ESBT)
                        };
                        if(student.TrainingStatus == TrainingStatus.Completed)
                        {
                            terminalReport.S_DateEmployed = "";
                            terminalReport.D_Occupation = deploymentDetails.Occupation;
                            terminalReport.D_EmployerName = deploymentDetails.EmployerName;
                            terminalReport.D_EmployerAddress = deploymentDetails.EmployerAddress;
                            terminalReport.D_Classification = deploymentDetails.Classification;
                            terminalReport.D_Salary = deploymentDetails.Salary;
                        }
                        TerminalReportList.Add(terminalReport);
                    }

                    //Populate the Excel Fields
                    worksheet.Cells["A1"].LoadFromCollection(TerminalReportList, true);

                    //Export as Excel Sheet
                    var excelData = package.GetAsByteArray();
                    var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    var fileName = $"{Id}_Terminal_Report.xlsx";

                    return new FileContentResult(excelData, contentType) { FileDownloadName = fileName };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static double TrueColumnWidth(double width)
        {
            if (width < 1)
            {
                return (12.0 / 7) * width;
            }
            else
            {
                return width + (5.0 / 7);
            }
        }
    }
}
