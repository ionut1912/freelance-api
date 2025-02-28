using Frelance.Application.Repositories.ML;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Tensorflow;
using static Tensorflow.Binding;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Frelance.Infrastructure.Services.ML
{
    public class TensorFlowFaceComparisonService : IFaceComparisionService
    {
        private readonly Session _session;
        private readonly Graph _graph;
        private const string ModelUrl = "https://storage.googleapis.com/facenet-models/20180402-114759.pb";
        private const string ModelPath = "face_embedding_model.pb";
        private readonly int _expectedWidth;
        private readonly int _expectedHeight;

        public TensorFlowFaceComparisonService(int expectedWidth = 160, int expectedHeight = 160)
        {
            _expectedWidth = expectedWidth;
            _expectedHeight = expectedHeight;

            if (!File.Exists(ModelPath))
            {
                using (var client = new HttpClient())
                {
                    var modelData = client.GetByteArrayAsync(ModelUrl).Result;
                    File.WriteAllBytes(ModelPath, modelData);
                }
            }
            _graph = new Graph();
            var model = File.ReadAllBytes(ModelPath);
            _graph.Import(model);
            _session = new Session(_graph);
        }

        public async Task<double> CompareFacesAsync(string base64Face1, string base64Face2)
        {
            var embedding1 = GetFaceEmbedding(base64Face1);
            var embedding2 = GetFaceEmbedding(base64Face2);
            var similarity = ComputeCosineSimilarity(embedding1, embedding2);
            return await Task.FromResult(similarity);
        }

        private float[] GetFaceEmbedding(string base64Image)
        {
            var imageBytes = Convert.FromBase64String(base64Image);
            using var image = Image.Load<Rgba32>(imageBytes);
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(_expectedWidth, _expectedHeight),
                Mode = ResizeMode.Crop
            }));

            var imageData = new float[_expectedWidth * _expectedHeight * 3];
            var index = 0;

            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    var pixel = image[x, y];
                    imageData[index++] = (pixel.R - 127.5f) / 128f;
                    imageData[index++] = (pixel.G - 127.5f) / 128f;
                    imageData[index++] = (pixel.B - 127.5f) / 128f;
                }
            }

            var tensor = tf.constant(imageData, shape: new long[] { 1, _expectedWidth, _expectedHeight, 3 });
            var inputOp = _graph.OperationByName("input");
            var embeddingsOp = _graph.OperationByName("embeddings");
            var result = _session.run(embeddingsOp.outputs[0], new FeedItem(inputOp.outputs[0], tensor));
            var embedding = result.ToArray<float>();
            return embedding;
        }

        private static double ComputeCosineSimilarity(float[] vectorA, float[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
                throw new ArgumentException("Vectors must have the same dimensions.");
            double dot = 0.0, normA = 0.0, normB = 0.0;
            for (var i = 0; i < vectorA.Length; i++)
            {
                dot += vectorA[i] * vectorB[i];
                normA += Math.Pow(vectorA[i], 2);
                normB += Math.Pow(vectorB[i], 2);
            }
            return dot / (Math.Sqrt(normA) * Math.Sqrt(normB));
        }
    }
}
