namespace HIKVISIONTestProj
{
    public class AcsEventResponse
    {
        public AcsEventModel AcsEvent { get; set; }
    }

    public class AcsEventModel
    {
        public string SearchID { get; set; }
        public int TotalMatches { get; set; }
        public string ResponseStatusStrg { get; set; }
        public int NumOfMatches { get; set; }
        public List<Info> InfoList { get; set; }
    }

    public class Info
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public DateTime Time { get; set; }
        public int? DoorNo { get; set; }
        public int? CardType { get; set; }
        public string Type { get; set; }
        public int SerialNo { get; set; }
        public string CurrentVerifyMode { get; set; }
        public string Mask { get; set; }
        public string RemoteHostAddr { get; set; }
        public string Name { get; set; }
        public int? CardReaderNo { get; set; }
        public string EmployeeNoString { get; set; }
        public string UserType { get; set; }
        public string AttendanceStatus { get; set; }
        public string Label { get; set; }
    }

}
