using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace AWSLambdaDemo.Controllers
{
    [Route("api/[controller]")]
    public class CelebritiesController : ControllerBase
    {
        /// <summary>
        /// The default minimum confidence used for detecting labels.
        /// </summary>
        public const float DEFAULT_MIN_CONFIDENCE = 75f;

        float MinConfidence { get; set; } = DEFAULT_MIN_CONFIDENCE;

        IAmazonRekognition RekognitionClient { get; }

        public CelebritiesController()
        {
            this.RekognitionClient = new AmazonRekognitionClient();
        }

        // POST api/celebrities
        [HttpPost]
        public IActionResult Post([FromForm] IFormFile file)
        {
            try
            {            
                using (var memoryStream = new MemoryStream())
                {
                    file.OpenReadStream().CopyTo(memoryStream);

                    Console.WriteLine($"Looking for celebrities in image {file.FileName} ...");

                    var recognizeCelebritiesResponse = this.RekognitionClient.RecognizeCelebritiesAsync(new RecognizeCelebritiesRequest
                    {
                        Image = new Amazon.Rekognition.Model.Image
                        {
                            Bytes = memoryStream
                        }
                    }).GetAwaiter().GetResult();

                    Console.WriteLine($"\tRecognized {recognizeCelebritiesResponse.CelebrityFaces.Count} celebrity(s).");

                    var celebrities = new Dictionary<string, string>();

                    foreach (Celebrity celebrity in recognizeCelebritiesResponse.CelebrityFaces)
                    {
                        Console.WriteLine($"\t\tCelebrity recognized: {celebrity.Name}");
                        Console.WriteLine($"\t\tCelebrity ID: {celebrity.Id}");

                        BoundingBox boundingBox = celebrity.Face.BoundingBox;
                        Console.WriteLine($"\t\tposition: " +
                           boundingBox.Left + " " + boundingBox.Top);
                        
                        Console.WriteLine("\t\tFurther information/URL(s) (if available):");
                        foreach (String url in celebrity.Urls)
                            Console.WriteLine($"\t\t{url}");

                        Console.WriteLine("\t\t---");

                        celebrities.Add(celebrity.Name, celebrity.MatchConfidence.ToString());
                    }
                    Console.WriteLine($"\t{recognizeCelebritiesResponse.UnrecognizedFaces.Count} face(s) were unrecognized.");

                    return Ok(celebrities);
                }
            }
            catch (Exception e)
            {
                var errorMsg = $"File length: {file.Length} Content-Type: {file.ContentType} Error: {e.Message}";
                return Problem(detail: errorMsg, statusCode: 500);
            }
        }
    }
}