using System.Xml;

public interface ISave {
	XmlNode Save();
	void Load(XmlNode data);
}
