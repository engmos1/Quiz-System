using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Enums
{
    public enum ErrorCode
    {
        // ===========================
        // Base / General
        // ===========================
        NoError = 0,
        UnexpectedError = 1,
        ValidationFailed = 2,

        // ===========================
        // User (1000 – 1099)
        // ===========================
        UserNotFound = 1000,
        UserAlreadyExists = 1001,
        UserCreationFailed = 1002,
        UserUpdateFailed = 1003,
        UserDeleteFailed = 1004,

        // ===========================
        // Student (1100 – 1199)
        // ===========================
        StudentNotFound = 1100,
        StudentAlreadyExists = 1101,
        StudentEnrollmentFailed = 1102,
        StudentHasNoCourses = 1103,

        // ===========================
        // Instructor (1200 – 1299)
        // ===========================
        InstructorNotFound = 1200,
        InstructorAlreadyExists = 1201,
        InstructorAssignmentFailed = 1202,

        // ===========================
        // Course (1300 – 1399)
        // ===========================
        CourseNotFound = 1300,
        CourseAlreadyExists = 1301,
        CourseCreationFailed = 1302,
        CourseUpdateFailed = 1303,
        CourseDeleteFailed = 1304,
        CourseHasNoInstructor = 1305,
        CourseHasNoStudents = 1306,

        // ===========================
        // PreRequest / PreRequisite (1400 – 1499)
        // ===========================
        PreRequisiteNotFound = 1400,
        PreRequisiteInvalid = 1401,
        PreRequisiteConflict = 1402,

        // ===========================
        // Exam (1500 – 1599)
        // ===========================
        ExamNotFound = 1500,
        ExamCreationFailed = 1501,
        ExamAlreadyExists = 1502,
        ExamHasNoQuestions = 1503,

        // ===========================
        // ExamQuestion (1600 – 1699)
        // ===========================
        ExamQuestionNotFound = 1600,
        ExamQuestionAlreadyExists = 1601,
        ExamQuestionInvalid = 1602,

        // ===========================
        // Question (1700 – 1799)
        // ===========================
        QuestionNotFound = 1700,
        QuestionAlreadyExists = 1701,
        QuestionInvalid = 1702,
        QuestionHasNoChoices = 1703,

        // ===========================
        // Choice (1800 – 1899)
        // ===========================
        ChoiceNotFound = 1800,
        ChoiceInvalid = 1801,
        ChoiceAlreadyExists = 1802,

        // ===========================
        // StudentExam (1900 – 1999)
        // ===========================
        StudentExamNotFound = 1900,
        StudentExamAlreadyExists = 1901,
        StudentExamInvalid = 1902,
        StudentExamSubmissionFailed = 1903,

        // ===========================
        // StudentAnswer (2000 – 2099)
        // ===========================
        StudentAnswerNotFound = 2000,
        StudentAnswerInvalid = 2001,
        StudentAnswerAlreadyExists = 2002,

        // ===========================
        // StudentCourse (2100 – 2199)
        // ===========================
        StudentCourseNotFound = 2100,
        StudentCourseAlreadyExists = 2101,
        StudentCourseRegistrationFailed = 2102,

        // ===========================
        // RoleFeature (2200 – 2299)
        // ===========================
        RoleFeatureNotFound = 2200,
        RoleFeatureAlreadyExists = 2201,
        RoleFeatureInvalid = 2202
    }

}
