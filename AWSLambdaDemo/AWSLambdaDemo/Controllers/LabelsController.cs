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
    public class LabelsController : ControllerBase
    {
        /// <summary>
        /// The default minimum confidence used for detecting labels.
        /// </summary>
        public const float DEFAULT_MIN_CONFIDENCE = 75f;

        float MinConfidence { get; set; } = DEFAULT_MIN_CONFIDENCE;

        IAmazonRekognition RekognitionClient { get; }

        public LabelsController()
        {
            this.RekognitionClient = new AmazonRekognitionClient();
        }

        // POST api/labels
        [HttpPost]
        public IActionResult Post([FromForm] IFormFile file)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.OpenReadStream().CopyTo(memoryStream);

                    Console.WriteLine($"Looking for labels in image {file.FileName} ...");

                    var detectResponses = this.RekognitionClient.DetectLabelsAsync(new DetectLabelsRequest
                    {
                        MinConfidence = MinConfidence,
                        Image = new Amazon.Rekognition.Model.Image
                        {
                            Bytes = memoryStream
                        }
                    }).GetAwaiter().GetResult();

                    var labels = new Dictionary<string, string>();

                    foreach (var label in detectResponses.Labels)
                    {
                        if (labels.Count < 10)
                        {
                            Console.WriteLine($"\tFound Label {label.Name} with confidence {label.Confidence}");
                            labels.Add(label.Name, label.Confidence.ToString());
                        }
                        else
                        {
                            Console.WriteLine($"\tSkipped label {label.Name} with confidence {label.Confidence} because the maximum number of tags has been reached");
                        }
                    }

                    return Ok(labels);
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