// See https://aka.ms/new-console-template for more information

using ConsoleApp1;
using OfficeOpenXml;

Console.WriteLine("Hello, World!");
var path = @"E:\testdaten.xlsx";

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var names = new[]
{
    "Viktor Axelsen",
    "Kento Momota",
    "Anders Antonsen",
    "Loh Kean Yew",
    "Akane Yamaguchi",
    "Mark Lamsfuss",
    "Gabrielle Adcock",
    "Chris Adcock"
};

var rackets = new[]
{
    "Yonex Arcsaber 10",
    "Yonex Nanoray 10",
    "Yonex Voltric 5",
    "Yonex Voltric Glan Z",
    "Babolat Satelite Gravity 74",
    "Victor Thruster Lightfighter 30",

    "Victor driveX 9x",
    "Victor Brave Sword 12",
    "Yonex NanoRay 800",
    "BABOLAT X Feel Power 2016",
    "OLIVER DELTA 7",

    "OLIVER RS ENTREX 100",
    "Victor V-4400 Magan",
};

var strings = new[]
{
    "Victor VBS-70",
    "Yonex BG-65",
    "Yonex BG-80",
    "Yonex Nanogy 95"
};

int numberOfPlayers = 30;
int numberOfJobs = 100;
int minRacketsPerPlayer = 3;
int maxRacketsPerPlayer = 8;
int maxStringsPerRacket = 5;

var rand = new Random();

var customers = new List<Player>();

foreach (var name in names)
{
    var p = new Player { Name = name };

    var racketCount = rand.Next(minRacketsPerPlayer, maxRacketsPerPlayer);

    for (var i = 0; i < racketCount; i++)
    {
        var racket = new Racket();

        string racketName;
        do
        {
            racketName = rackets[rand.Next(0, rackets.Length)];
        } while (p.Rackets.Any(x => x.Name == racketName));

        racket.Name = racketName;

        var stringCount = rand.Next(1, maxStringsPerRacket);
        for (var j = 0; j < stringCount; j++)
        {
            string stringName;
            do
            {
                stringName = strings[rand.Next(0, strings.Length)];
            } while (racket.Strings.Any(x => x == stringName));
            racket.Strings.Add(stringName);
        }

        p.Rackets.Add(racket);
    }

    customers.Add(p);
}

Console.WriteLine(customers.Count);

var package = new ExcelPackage();
var worksheet = package.Workbook.Worksheets.Add("Testdaten");

worksheet.Cells[1, 1].Value = "Datum";
worksheet.Cells[1, 2].Value = "Name";
worksheet.Cells[1, 3].Value = "Schläger";
worksheet.Cells[1, 4].Value = "Saite";
worksheet.Cells[1, 5].Value = "Bespannung";
worksheet.Cells[1, 6].Value = "Bezahlt";
worksheet.Cells[1, 7].Value = "Fertig";
worksheet.Cells[1, 8].Value = "Kommentar";

var date = DateOnly.FromDateTime(DateTime.Today);

for (var i = 0; i < numberOfJobs; i++)
{
    var row = i + 2;

    var player = customers[rand.Next(0, customers.Count)];
    var racket = player.Rackets[rand.Next(0, player.Rackets.Count)];
    var stringing = racket.Strings[rand.Next(0, racket.Strings.Count)];

    var comment = string.Empty;
    var randomNumber = rand.Next(0, 10);
    if (randomNumber >= 8)
        comment = "Haarriß im Schläger";
    else if (randomNumber >= 6)
        comment = "Riss im Schaft";
    else if (randomNumber >= 5)
        comment = "Neue Saite zum testen";
    

    worksheet.Cells[row, 1].Value = date.ToString("dd.MM.yyyy");
    worksheet.Cells[row, 2].Value = player.Name;
    worksheet.Cells[row, 3].Value = racket.Name;
    worksheet.Cells[row, 4].Value = stringing;
    worksheet.Cells[row, 5].Value = (rand.Next(100, 180) / 10d).ToString("F1");
    worksheet.Cells[row, 6].Value = rand.Next(0, 10) > 8 ? 0 : 1;
    worksheet.Cells[row, 7].Value = rand.Next(0, 10) > 8 ? 0 : 1;
    worksheet.Cells[row, 8].Value = comment;

    date = date.AddDays(-1 * rand.Next(0, 14));
}

worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
package.SaveAs(path);