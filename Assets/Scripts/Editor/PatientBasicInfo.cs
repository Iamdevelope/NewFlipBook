using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public class Request
{
    public string System { get; set; }
    public string SecurityCode { get; set; }
    public PatientBasicInfo PatientInfo { get; set; }
}

[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PatientBasicInfo
{
    
}