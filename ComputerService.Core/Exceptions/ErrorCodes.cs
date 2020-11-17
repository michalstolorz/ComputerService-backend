namespace ComputerService.Core.Exceptions
{
    public static class ErrorCodes
    {
        //Auth
        public static string PrivateKeyNotFound => "private_key_not_found";
        public static string IdNotFound => "id_not_found";

        //Helpers
        public static string PermissionNotDefined => "permission_not_defined";

        //Validator
        public static string InvalidParameter => "invalid_parameter";
        public static string InvalidFile => "invalid_file";

        //Repository
        public static string PredicateCannotBeNull => "parameter_predicate_cannot_be_null";
        public static string SelectCannotBeNull => "parameter_select_cannot_be_null";
        public static string EntitiestCannotBeNull => "parameter_entities_cannot_be_null";
        public static string IdCannotBeNullOrZero => "parameter_id_cannot_be_null_or_zero";
        public static string ObjectOfTypeNotFound => "object_of_type_not_found";
        public static string InvalidIdType => "invalid_id_type";

        //Service
        public static string RequestCannotBeNull => "request_cannot_be_null";
        public static string SettingNotFound => "settings_not_found";
        public static string PositionSkillNotFound => "positionskill_not_found";
        public static string UserWithGivenIdNotFound => "user_with_given_id_not_found";
        public static string DepartmentWithGivenIdNotFound => "department_with_given_id_not_found";
        public static string InterviewWithGivenIdNotFound => "interviewdata_with_given_id_not_found";
        public static string PositionWithGivenIdNotFound => "position_with_given_id_not_found";
        public static string ReferrerWithGivenIdNotFound => "referrer_with_given_id_not_found";
        public static string RecruitmentWithGivenIdNotFound => "recruitment_with_given_id_not_found";
        public static string RecruitmentCandidatetWithGivenIdNotFound => "recruitment_candidate_with_given_id_not_found";
        public static string StatusWithGivenIdNotFound => "status_with_given_id_not_found";
        public static string PositionSkillWithGivenIdNotFound => "positionskill_with_given_id_not_found";
        public static string DepartmentRecruitmentWithGivenIdNotFound => "department_recruitment_with_given_id_not_found";
        public static string TypeOfContractWithGivenIdNotFound => "typeofcontract_with_given_id_not_found";
        public static string DepartmentAlreadyExists => "department_already_exists";
        public static string PositionAlreadyExists => "position_already_exists";
        public static string ReferrerAlreadyExists => "referrer_already_exists";
        public static string SkillAlreadyExists => "skill_already_exists";
        public static string CandidateAlreadyExists => "candidate_already_exists";
        public static string TypeOfContractAlreadyExists => "typeofcontract_already_exists";
        public static string EmailError => "email_error";
        public static string EmailInvalidType => "invalid_email_type";
        public static string UserNotAssignedToRecruitment => "user_not_assigned_to_recruitment";
        public static string PositionSkillAlreadyExists => "position_skill_already_exists";
        public static string InvalidTemplateName => "invalid_template_name";
        public static string UserRecruitmentAlreadyExist => "user_recruitment_already_exists";
        public static string DepartmentRecruitmentAlreadyExist => "department_recruitment_already_exists";
        public static string CandidateWithGivenIdNotFound => "candidate_with_given_id_not_found";
        public static string InsufficientPermissions => "insufficient_permissions";
        public static string RecruitmentCandidateRateWithGivenIdNotFound => "recruitment_candidate_rate_with_given_id_not_found";
    }
}
