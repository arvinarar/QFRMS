using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using QFRMS.Data.Interfaces;
using QFRMS.Data.ViewModels;
using QFRMS.Services.Interfaces;
using QFRMS.Services.Utils;
using static QFRMS.Data.Enums.EnumHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFRMS.Data.DTOs;
using static QFRMS.Data.Constants;
using QFRMS.Data.Models;
using System.Globalization;

namespace QFRMS.Services.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        private readonly IBatchRepository _batchRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserAccountRepository _userRepository;
        private readonly ILogger<StudentService> _logger;
        private readonly Work _work = new Work();

        public StudentService(IStudentRepository repository, IBatchRepository batchRepository, ICourseRepository courseRepository, IUserAccountRepository userAccountRepository, ILogger<StudentService> logger)
        {
            _repository = repository;
            _batchRepository = batchRepository;
            _courseRepository = courseRepository;
            _userRepository = userAccountRepository;
            _logger = logger;
        }

        public async Task<IQueryable<StudentListViewModel>> GetStudentListAsync(string? TrainorName)
        {
            try
            {
                if(TrainorName.IsNullOrEmpty())
                {
                    return await Task.FromResult(from student in await _repository.RetrieveAllAsync()
                                                 orderby student.LastName, student.FirstName
                                                 let contactNo = student.ContactNo ?? "N/A"
                                                 let email = student.Email ?? "N/A"
                                                 select new StudentListViewModel
                                                 {
                                                     ULI = student.ULI,
                                                     Name = $"{student.FirstName} {student.MiddleName[0]}. {student.LastName} {student.ExtensionName}",
                                                     Age = student.Age.ToString(),
                                                     ContactNo = contactNo,
                                                     Email = email,
                                                     HighestGrade = GetEnumDescription(student.HighestGrade),
                                                     Status = GetEnumDescription(student.TrainingStatus)
                                                 });
                }
                else
                {
                    return await Task.FromResult(from student in await _repository.RetrieveAllAsync()
                                                 join batch in await _batchRepository.RetrieveAllAsync() on student.BatchId equals batch.Id
                                                 join trainor in await _userRepository.GetUsersAsync() on batch.TrainorId equals trainor.Id
                                                 where trainor.UserName == TrainorName
                                                 orderby student.LastName, student.FirstName
                                                 let contactNo = student.ContactNo ?? "N/A"
                                                 let email = student.Email ?? "N/A"
                                                 select new StudentListViewModel
                                                 {
                                                     ULI = student.ULI,
                                                     Name = $"{student.FirstName} {student.MiddleName[0]}. {student.LastName} {student.ExtensionName}",
                                                     Age = student.Age.ToString(),
                                                     ContactNo = contactNo,
                                                     Email = email,
                                                     HighestGrade = GetEnumDescription(student.HighestGrade),
                                                     Status = GetEnumDescription(student.TrainingStatus)
                                                 });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime} GetStudentListAsync Failed: {message}", DateTime.Now.ToString(), ex.Message);
                return await Task.FromResult(Enumerable.Empty<StudentListViewModel>().AsQueryable());
            }
        }
        public async Task<IQueryable<StudentListViewModel>> SearchStudentListAsync(string searchType, string searchInput, string? TrainorName)
        {
            try
            {
                if (TrainorName.IsNullOrEmpty())
                {
                    return searchType switch 
                    {      
                        "ULI" => from student in await _repository.RetrieveAllAsync()
                                 orderby student.LastName, student.FirstName
                                 where student.ULI.Contains(searchInput)
                                 let contactNo = student.ContactNo ?? "N/A"
                                 let email = student.Email ?? "N/A"
                                 select new StudentListViewModel
                                 {
                                     ULI = student.ULI,
                                     Name = $"{student.FirstName} {student.MiddleName[0]}. {student.LastName} {student.ExtensionName}",
                                     Age = student.Age.ToString(),
                                     ContactNo = contactNo,
                                     Email = email,
                                     HighestGrade = GetEnumDescription(student.HighestGrade),
                                     Status = GetEnumDescription(student.TrainingStatus)
                                 },

                        "Name" => from student in await _repository.RetrieveAllAsync()
                                  orderby student.LastName, student.FirstName
                                  where 
                                    student.FirstName.Contains(searchInput) ||
                                    student.MiddleName.Contains(searchInput) ||
                                    student.LastName.Contains(searchInput) ||
                                    student.ExtensionName == searchInput
                                  let contactNo = student.ContactNo ?? "N/A"
                                  let email = student.Email ?? "N/A"
                                  select new StudentListViewModel
                                  {
                                      ULI = student.ULI,
                                      Name = $"{student.FirstName} {student.MiddleName[0]}. {student.LastName} {student.ExtensionName}",
                                      Age = student.Age.ToString(),
                                      ContactNo = contactNo,
                                      Email = email,
                                      HighestGrade = GetEnumDescription(student.HighestGrade),
                                      Status = GetEnumDescription(student.TrainingStatus)
                                  },

                        _ => from student in await _repository.RetrieveAllAsync()
                             orderby student.LastName, student.FirstName
                             let contactNo = student.ContactNo ?? "N/A"
                             let email = student.Email ?? "N/A"
                             select new StudentListViewModel
                             {
                                 ULI = student.ULI,
                                 Name = $"{student.FirstName} {student.MiddleName[0]}. {student.LastName} {student.ExtensionName}",
                                 Age = student.Age.ToString(),
                                 ContactNo = contactNo,
                                 Email = email,
                                 HighestGrade = GetEnumDescription(student.HighestGrade),
                                 Status = GetEnumDescription(student.TrainingStatus)
                             }
                    };
                }
                else
                {
                    return searchType switch
                    {
                        "ULI" => from student in await _repository.RetrieveAllAsync()
                                 join batch in await _batchRepository.RetrieveAllAsync() on student.BatchId equals batch.Id
                                 join trainor in await _userRepository.GetUsersAsync() on batch.TrainorId equals trainor.Id
                                 orderby student.LastName, student.FirstName
                                 where
                                    trainor.UserName == TrainorName &&
                                    student.ULI.Contains(searchInput)
                                 let contactNo = student.ContactNo ?? "N/A"
                                 let email = student.Email ?? "N/A"
                                 select new StudentListViewModel
                                 {
                                     ULI = student.ULI,
                                     Name = $"{student.FirstName} {student.MiddleName[0]}. {student.LastName} {student.ExtensionName}",
                                     Age = student.Age.ToString(),
                                     ContactNo = contactNo,
                                     Email = email,
                                     HighestGrade = GetEnumDescription(student.HighestGrade),
                                     Status = GetEnumDescription(student.TrainingStatus)
                                 },

                        "Name" => from student in await _repository.RetrieveAllAsync()
                                  join batch in await _batchRepository.RetrieveAllAsync() on student.BatchId equals batch.Id
                                  join trainor in await _userRepository.GetUsersAsync() on batch.TrainorId equals trainor.Id
                                  orderby student.LastName, student.FirstName
                                  where
                                    trainor.UserName == TrainorName &&
                                    student.FirstName.Contains(searchInput) ||
                                    student.MiddleName.Contains(searchInput) ||
                                    student.LastName.Contains(searchInput) ||
                                    student.ExtensionName == searchInput
                                  let contactNo = student.ContactNo ?? "N/A"
                                  let email = student.Email ?? "N/A"
                                  select new StudentListViewModel
                                  {
                                      ULI = student.ULI,
                                      Name = $"{student.FirstName} {student.MiddleName[0]}. {student.LastName} {student.ExtensionName}",
                                      Age = student.Age.ToString(),
                                      ContactNo = contactNo,
                                      Email = email,
                                      HighestGrade = GetEnumDescription(student.HighestGrade),
                                      Status = GetEnumDescription(student.TrainingStatus)
                                  },

                        _ => from student in await _repository.RetrieveAllAsync()
                             join batch in await _batchRepository.RetrieveAllAsync() on student.BatchId equals batch.Id
                             join trainor in await _userRepository.GetUsersAsync() on batch.TrainorId equals trainor.Id
                             where trainor.UserName == TrainorName
                             orderby student.LastName, student.FirstName
                             let contactNo = student.ContactNo ?? "N/A"
                             let email = student.Email ?? "N/A"
                             select new StudentListViewModel
                             {
                                 ULI = student.ULI,
                                 Name = $"{student.FirstName} {student.MiddleName[0]}. {student.LastName} {student.ExtensionName}",
                                 Age = student.Age.ToString(),
                                 ContactNo = contactNo,
                                 Email = email,
                                 HighestGrade = GetEnumDescription(student.HighestGrade),
                                 Status = GetEnumDescription(student.TrainingStatus)
                             }
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime} SearchStudentListAsync Failed: {message}", DateTime.Now.ToString(), ex.Message);
                return await Task.FromResult(Enumerable.Empty<StudentListViewModel>().AsQueryable());
            }
        }

        public async Task<IQueryable<StudentListViewModel>> SearchStudentBatchListAsync(string batchId, string searchType, string searchInput)
        {
            try
            {
                return searchType switch
                {
                    "ULI" => from student in await _repository.RetrieveStudentsFromBatchAsync(batchId)
                             orderby student.LastName, student.FirstName
                             where
                                student.ULI.Contains(searchInput)
                             let contactNo = student.ContactNo ?? "N/A"
                             let email = student.Email ?? "N/A"
                             select new StudentListViewModel
                             {
                                 ULI = student.ULI,
                                 Name = $"{student.FirstName} {student.MiddleName[0]}. {student.LastName} {student.ExtensionName}",
                                 Age = student.Age.ToString(),
                                 ContactNo = contactNo,
                                 Email = email,
                                 HighestGrade = GetEnumDescription(student.HighestGrade),
                                 Status = GetEnumDescription(student.TrainingStatus)
                             },

                    "Name" => from student in await _repository.RetrieveStudentsFromBatchAsync(batchId)
                              orderby student.LastName, student.FirstName
                              where
                                student.FirstName.Contains(searchInput) ||
                                student.MiddleName.Contains(searchInput) ||
                                student.LastName.Contains(searchInput) ||
                                student.ExtensionName == searchInput
                              let contactNo = student.ContactNo ?? "N/A"
                              let email = student.Email ?? "N/A"
                              select new StudentListViewModel
                              {
                                  ULI = student.ULI,
                                  Name = $"{student.FirstName} {student.MiddleName[0]}. {student.LastName} {student.ExtensionName}",
                                  Age = student.Age.ToString(),
                                  ContactNo = contactNo,
                                  Email = email,
                                  HighestGrade = GetEnumDescription(student.HighestGrade),
                                  Status = GetEnumDescription(student.TrainingStatus)
                              },

                    _ => from student in await _repository.RetrieveStudentsFromBatchAsync(batchId)
                         orderby student.LastName, student.FirstName
                         let contactNo = student.ContactNo ?? "N/A"
                         let email = student.Email ?? "N/A"
                         select new StudentListViewModel
                         {
                             ULI = student.ULI,
                             Name = $"{student.FirstName} {student.MiddleName[0]}. {student.LastName} {student.ExtensionName}",
                             Age = student.Age.ToString(),
                             ContactNo = contactNo,
                             Email = email,
                             HighestGrade = GetEnumDescription(student.HighestGrade),
                             Status = GetEnumDescription(student.TrainingStatus)
                         }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime} SearchStudentBatchListAsync Failed: {message}", DateTime.Now.ToString(), ex.Message);
                return await Task.FromResult(Enumerable.Empty<StudentListViewModel>().AsQueryable());
            }
        }

        public async Task<EnrollStudent> CreateStudentDTOAsync(string? batchId, bool FromCoursePage = false)
        {
            try
            {
                var batchlist = (from batch in await _batchRepository.RetrieveAllAsync()
                                 orderby batch.Id
                                 select new BatchList
                                 {
                                     RQMCode = batch.Id
                                 });
                return new EnrollStudent
                {
                    BatchId = batchId ?? null,
                    BatchList = [.. batchlist],
                    FromCoursePage = FromCoursePage
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<EditStudent> EditStudentDTOAsync(string ULI, string? batchId, bool FromCoursePage = false)
        {
            try
            {
                var student = await _repository.GetStudentAsync(ULI) ?? throw new NullReferenceException("Student not Found");
                var batchlist = (from batch in await _batchRepository.RetrieveAllAsync()
                                 orderby batch.Id
                                 select new BatchList
                                 {
                                     RQMCode = batch.Id
                                 });
                return new EditStudent
                {
                    ULI = student.ULI,
                    BatchId = student.BatchId,
                    FirstName = student.FirstName,
                    MiddleName = student.MiddleName,
                    LastName = student.LastName,
                    ExtensionName = student.ExtensionName,
                    ContactNo = student.ContactNo,
                    Email = student.Email,
                    StreetNo = student.StreetNo,
                    Barangay = student.Barangay,
                    MunicipalityCity = student.MunicipalityCity,
                    District = student.District,
                    Province = student.Province,
                    Sex = student.Sex,
                    BirthDate = student.BirthDate,
                    Age = student.Age,
                    CivilStatus = student.CivilStatus,
                    HighestGrade = student.HighestGrade,
                    Nationality = student.Nationality!,
                    TrainingStatus = student.TrainingStatus,
                    ESBT = student.ESBT,
                    BatchList = [.. batchlist],
                    FromCoursePage = FromCoursePage,
                    FromStudentsPage = batchId.IsNullOrEmpty(),
                    CurrentBatchId = student.BatchId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Work> EnrollStudentAsync(EnrollStudent model)
        {
            try
            {
                //Check if ULI already exist
                if (await _repository.GetStudentAsync(model.ULI!) != null) throw new ArgumentException("Student with Learner's ID already exist");
                var work = await _repository.CreateStudentAsync(new Student
                {
                    ULI = model.ULI!,
                    BatchId = model.BatchId!,
                    Batch = await _batchRepository.GetBatchAsync(model.BatchId!) ?? throw new NullReferenceException("Batch with RQM Code doesn't Exist"),
                    FirstName = model.FirstName!,
                    MiddleName = model.MiddleName!,
                    LastName = model.LastName!,
                    ExtensionName = model.ExtensionName,
                    ContactNo = model.ContactNo,
                    Email = model.Email,
                    StreetNo = model.StreetNo,
                    Barangay = model.Barangay!,
                    MunicipalityCity = model.MunicipalityCity!,
                    District = model.District,
                    Province = model.Province!,
                    Sex = model.Sex!.Value,
                    BirthDate = model.BirthDate!.Value,
                    Age = model.Age!.Value,
                    CivilStatus = model.CivilStatus!.Value,
                    HighestGrade = model.HighestGrade!.Value,
                    Nationality = model.Nationality!,
                    TrainingStatus = model.TrainingStatus!.Value,
                    ESBT = model.ESBT!.Value
                });
                if(!work) throw new Exception("Work failed");
                _work.Time = DateTime.Now;
                _work.Message = $"Successfully Enrolled Student \'{model.FirstName} {model.MiddleName} {model.LastName} {model.ExtensionName}\'";
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
                _work.Message = "Couldn't Enroll Student";
                _work.Result = false;
                return _work;
            }
        }

        public async Task<Work> EditStudentAsync(EditStudent model)
        {
            try
            {
                var student = await _repository.GetStudentAsync(model.ULI!) ?? throw new ArgumentException("Student with Learner's ID already exist");

                student.BatchId = model.BatchId!;
                student.FirstName = model.FirstName!;
                student.MiddleName = model.MiddleName!;
                student.LastName = model.LastName!;
                student.ExtensionName = model.ExtensionName;
                student.ContactNo = model.ContactNo;
                student.Email = model.Email;
                student.StreetNo = model.StreetNo;
                student.Barangay = model.Barangay!;
                student.MunicipalityCity = model.MunicipalityCity!;
                student.District = model.District;
                student.Province = model.Province!;
                student.Sex = model.Sex!.Value;
                student.BirthDate = model.BirthDate!.Value;
                student.Age = model.Age!.Value;
                student.CivilStatus = model.CivilStatus!.Value;
                student.HighestGrade = model.HighestGrade!.Value;
                student.Nationality = model.Nationality!;
                student.TrainingStatus = model.TrainingStatus!.Value;
                student.ESBT = model.ESBT!.Value;

                var work = await _repository.UpdateStudentAsync(student);
                if (!work) throw new Exception("Work failed");
                _work.Time = DateTime.Now;
                _work.Message = $"Successfully Updated Student \'{model.FirstName} {model.MiddleName} {model.LastName} {model.ExtensionName}\'";
                _work.Result = true;
                return _work;
            }
            catch (Exception ex)
            {
                _work.ErrorCode = ex.Message;
                _work.Time = DateTime.Now;
                _work.Message = "Couldn't Update Student";
                _work.Result = false;
                return _work;
            }
        }

        public async Task<Work> DeleteStudentAsync(string ULI)
        {
            try
            {
                var work = await _repository.DeleteStudentAsync(ULI);
                if (!work) throw new Exception("Work failed");

                _work.Time = DateTime.Now;
                _work.Message = $"Successfully Deleted Student \'{ULI}\'";
                _work.Result = true;
                return _work;

            }
            catch (Exception ex)
            {
                _work.ErrorCode = ex.Message;
                _work.Time = DateTime.Now;
                _work.Message = "Couldn't Delete Student";
                _work.Result = false;
                return _work;
            }
        }

        public async Task<Work> UpdateGrades(StudentGradesList model)
        {
            try
            {
                //Update Grades only
                if(model.IsTrainor)
                {
                    var grades = await _repository.RetrieveAllGradesAsync();
                    foreach (var student in model.Students)
                    {
                        grades.Where(g => g.ULI == student.ULI).FirstOrDefault()!.PreTest = student.Pretest;
                        grades.Where(g => g.ULI == student.ULI).FirstOrDefault()!.PostTest = student.Posttest;
                    }

                    var work = await _repository.UpdateGrades(grades);
                    if (!work) throw new Exception("Work failed");
                    _work.Time = DateTime.Now;
                    _work.Message = $"Successfully Updated Grades of Batch \'{model.BatchId}\'";
                    _work.Result = true;
                    return _work;
                }
                else //Update Training Status
                {
                    var students = await _repository.RetrieveStudentsFromBatchAsync(model.BatchId);
                    foreach (var student in model.Students)
                    {
                        students.Where(s => s.ULI == student.ULI).FirstOrDefault()!.TrainingStatus = student.TrainingStatus;
                    }

                    var work = await _repository.UpdateTrainingStatus(students);
                    if (!work) throw new Exception("Work failed");
                    _work.Time = DateTime.Now;
                    _work.Message = $"Successfully Updated Training Statuses of Batch \'{model.BatchId}\'";
                    _work.Result = true;
                    return _work;
                }
            }
            catch (Exception ex)
            {
                _work.ErrorCode = ex.Message;
                _work.Time = DateTime.Now;
                _work.Message = "Couldn't Update Grades";
                _work.Result = false;
                return _work;
            }
        }

        public async Task<bool> CheckIfAlreadyExist(string ULI)
        {
            try
            {
                return await _repository.GetStudentAsync(ULI) != null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<StudentDetailViewModel> GetStudentDetail(string ULI, string? BatchId, bool fromCoursePage)
        {
            try
            {
                var s = _repository.RetrieveAllAsync().Result.Where(s => s.ULI == ULI) ?? throw new NullReferenceException("Student Not Found");
                var StudentDetails = (from student in s
                                      join batch in await _batchRepository.RetrieveAllAsync() on student.BatchId equals batch.Id
                                      join course in await _courseRepository.RetrieveAllAsync() on batch.CourseId equals course.Id
                                      select new StudentDetailViewModel
                                      {
                                          ULI = ULI,
                                          Name = $"{student.FirstName} {student.MiddleName} {student.LastName} {student.ExtensionName}",
                                          BirthDate = student.BirthDate.ToString("MMM dd, yyyy"),
                                          Age = student.Age,
                                          Sex = GetEnumDescription(student.Sex),
                                          CivilStatus = GetEnumDescription(student.CivilStatus),
                                          Nationality = student.Nationality,
                                          ContactNo = student.ContactNo,
                                          Email = student.Email,
                                          Address = $"{student.StreetNo}, {student.Barangay}, {student.MunicipalityCity}, {student.Province}",
                                          HighestGrade = GetEnumDescription(student.HighestGrade),
                                          ESBT = GetEnumDescription(student.ESBT),
                                          EnrolledProgram = course.ProgramTitle,
                                          TrainingStatus = GetEnumDescription(student.TrainingStatus),
                                          BatchId = BatchId,
                                          FromCoursePage = fromCoursePage
                                      }).First();
                if(StudentDetails.TrainingStatus != GetEnumDescription(TrainingStatus.Dropout))
                {
                    var grades = await _repository.GetStudentGradeAsync(s.First().ULI);
                    decimal? FinalGrade = null;
                    if (grades.PreTest.HasValue && grades.PostTest.HasValue) // Check if has grades to get final grade
                    {
                        var avg = (grades.PreTest.Value + grades.PostTest.Value) / 2;
                        FinalGrade = decimal.Round(avg, 2, MidpointRounding.AwayFromZero);
                    }
                    StudentDetails.PreTestGrade = grades.PreTest.HasValue ? $"{grades.PreTest.Value} / 100" : "TBA";
                    StudentDetails.PostTestGrade = grades.PostTest.HasValue ? $"{grades.PostTest.Value} / 100" : "TBA";
                    StudentDetails.FinalGrade = FinalGrade.HasValue ? $"{FinalGrade} / 100" : "TBA";
                }
                else
                {
                    StudentDetails.PreTestGrade = "Dropped";
                    StudentDetails.PostTestGrade = "Dropped";
                    StudentDetails.FinalGrade = "Dropped";
                }

                var deploymentDetail = await _batchRepository.GetDeploymentDetailsAsync(s.First().BatchId);
                if (deploymentDetail != null && StudentDetails.TrainingStatus == TrainingStatus.Completed.ToString())
                {
                    StudentDetails.Occupation = deploymentDetail.Occupation;
                    StudentDetails.Salary = deploymentDetail.Salary;
                    StudentDetails.Classification = deploymentDetail.Classification;
                    StudentDetails.EmployerName = deploymentDetail.EmployerName;
                    StudentDetails.EmployerAddress = deploymentDetail.EmployerAddress;
                }

                return StudentDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<StudentGradesList> GetStudentGrades(string batchId, bool isTrainor, bool fromCoursePage)
        {
            try
            {
                return new StudentGradesList
                {
                    IsTrainor = isTrainor,
                    FromCoursePage = fromCoursePage,
                    BatchId = batchId,
                    Students = (from student in await _repository.RetrieveAllAsync()
                                join grades in await _repository.RetrieveAllGradesAsync() on student.ULI equals grades.ULI
                                join batch in await _batchRepository.RetrieveAllAsync() on student.BatchId equals batch.Id
                                where batch.Id == batchId
                                let avg = (grades.PreTest.HasValue && grades.PostTest.HasValue) ? (grades.PreTest.Value + grades.PostTest.Value) / 2 : 0
                                let finalGrade = (grades.PreTest.HasValue && grades.PostTest.HasValue) ? $"{decimal.Round(avg, 2, MidpointRounding.AwayFromZero)} / 100" : "TBA"
                                orderby student.LastName
                                select new StudentGrade
                                {
                                    ULI = student.ULI,
                                    Name = $"{student.LastName} {student.ExtensionName}, {student.FirstName} {student.MiddleName.ToUpper()[0]}.",
                                    Pretest = grades.PreTest,
                                    Posttest = grades.PostTest,
                                    FinalGrade = finalGrade,
                                    TrainingStatus = student.TrainingStatus
                                }).ToList()
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
