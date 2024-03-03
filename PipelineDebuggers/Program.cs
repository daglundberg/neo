// See https://aka.ms/new-console-template for more information



var x = new NeoFontContentPipelineExtension.NeoFontImporter();

var f = x.Import(@"C:\Users\Osmund\Downloads\neo-master\neo-master\NeoTestApp.Code\Content\default_font_map.csv", null);
Console.WriteLine(f);