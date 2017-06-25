using System.Xml;

public interface ISave {
	XmlNode Save(XmlDocument xml);
	void Load(XmlNode data);
}
