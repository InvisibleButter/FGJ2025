using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot(ElementName = "map")]
public class MapConfig
{
    [XmlElement(ElementName = "entity")]
    public List<Entity> Entities { get; set; }

    [XmlRoot(ElementName = "entity")]
    public class Entity
    {
        [XmlAttribute(AttributeName = "x")]
        public int X { get; set; }

        [XmlAttribute(AttributeName = "y")]
        public int Y { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "width")]
        public int Width { get; set; }

        [XmlAttribute(AttributeName = "height")]
        public int Height { get; set; }
    }
}
