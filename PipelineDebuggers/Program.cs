// See https://aka.ms/new-console-template for more information


using System.Text.Json;
using System.Xml.Serialization;
using Neo;

var x = new NeoFontContentPipelineExtension.NeoFontImporter();

var f = x.Import("/Users/dag/Code/neo/NeoTestApp.Code/Content/default_font_map.csv", null);
