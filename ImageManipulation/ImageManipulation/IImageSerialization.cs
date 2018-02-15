using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation
{
    public interface IImageSerialization
    {
        string Serialize(Image img);
        Image Parse(string imageData);
    }
}
