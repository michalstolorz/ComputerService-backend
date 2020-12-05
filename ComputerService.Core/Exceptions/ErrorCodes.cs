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
        public static string RepairWithGivenIdNotFound => "repair_with_given_id_not_found";
        public static string CustomerWithGivenIdNotFound => "customer_with_given_id_not_found";
        public static string RepairTypeWithGivenIdNotFound => "repair_type_with_given_id_not_found";
        public static string RepairTypeAlreadyAssignToRepair => "repair_type_with_given_id_already_assign_to_given_repair";
        public static string RepairTypeAlreadyExists => "repair_type_already_exists";
        public static string PartAlreadyExists => "part_already_exists";
        public static string PartWithGivenIdNotFound => "part_with_given_id_not_found";
    }
}
