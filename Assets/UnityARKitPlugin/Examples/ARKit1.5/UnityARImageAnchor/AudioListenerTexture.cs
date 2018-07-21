using UnityEngine;
using System.Collections;

public class AudioListenerTexture : MonoBehaviour
{

    private int width; // texture width
    private int height; // texture height
    private Color backgroundColor = Color.black;
    //public Color waveformColor = Color.green;
    public int size = 1024; // size of sound segment displayed in texture

    private Color[] blank; // blank image array
    public Texture2D AudioTexture;
    public float[] samples; // audio samples array
    public float[] lowRes;
    public int lowResSize;// = 256;

    public ComputeBuffer _buffer;

    void OnEnable()
    {
        width = size;
        height = 1;

        // create the samples array
        samples = new float[size*8];
        lowRes  = new float[64];
        lowResSize = 64;

        // create the AudioTexture and assign to the guiTexture:
        AudioTexture = new Texture2D (width, height);

        // create a 'blank screen' image
        blank = new Color[width * height];

        _buffer = new ComputeBuffer(size, 4 * sizeof(float));

        for (int i = 0; i < blank.Length; i++) {
            blank [i] = backgroundColor;
        }

        // refresh the display each 100mS

    }

    void OnDisable(){
        _buffer.Release();
    }

    void Update(){
        GetCurWave();
    }

    void GetCurWave (){
        // clear the AudioTexture
       // AudioTexture.SetPixels (blank, 0);

        // get samples from channel 0 (left)
        //GetComponent<AudioListener>().GetOutputData (samples, 0);

        AudioListener.GetSpectrumData(samples, 0, FFTWindow.Triangle);
        //AudioListener.GetSpectrumData(lowRes, 0, FFTWindow.Triangle);

        //Color c;
        //float r , g, b, a;

        Color[] pixels = AudioTexture.GetPixels(0,0,width,1 );
        //print( pixels.Length );
        Color og;
        // draw the waveform
        for (int i = 0; i < size; i++) {

           // og = pixels[i];//AudioTexture.GetPixel((int)(width * i / size), (int)(1 * (samples [i])) - 1 );

            pixels[i].r = pixels[i].r * .8f + samples[ (int)(i * 4)  + 0 ] * 128;
            pixels[i].g = pixels[i].g * .8f + samples[ (int)(i * 4)  + 1 ] * 128;
            pixels[i].b = pixels[i].b * .8f + samples[ (int)(i * 4)  + 2 ] * 128;
            pixels[i].a = pixels[i].a * .8f + samples[ (int)(i * 4)  + 3 ] * 128;

           //pixels[i].Set(r, g, b, a);

            //AudioTexture.SetPixel((int)(width * i / size), (int)(1 * (samples [i])) - 1, c );
        } // upload to the graphics card

        AudioTexture.SetPixels(pixels);
        AudioTexture.Apply ();

        _buffer.SetData(samples );
        
    }
}