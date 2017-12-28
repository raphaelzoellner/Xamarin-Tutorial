using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Phoneword
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CircleSpin : ContentPage
    {
        // TODO: 
        // list players on screen
        // catch players with the same name
        // add slider to control number of fields per player on spinning wheel
        // remove any player not just last
        // prettify appearance on screen

        bool spinCircle = false;
        static Random rnd = new Random();
        Double totalSpinAmount = (1 + 2 * rnd.NextDouble())*360;
        Double spinAmount;
        float circleRadius = 500;

        List<string> participants = new List<string>();

        public CircleSpin()
        {
            InitializeComponent();
        }

        void OnAddPlayer(object sender, EventArgs args)
        {
            participants.Add(playerName.Text);
            CanvasView1.InvalidateSurface();
        }

        void OnDeleteLastPlayer(object sender, EventArgs args)
        {
            if (participants.Count != 0)
            {
                participants.RemoveAt(participants.Count - 1);
                CanvasView1.InvalidateSurface();
            }
        }

        void OnCanvasViewTapped(object sender, EventArgs args)
        {
            Animation(sender);
            spinCircle = true;
            (sender as SKCanvasView).InvalidateSurface();
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            int numberOfParticipants = participants.Count;
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 5,
                TextSize = 50
            };
            canvas.DrawLine(info.Width / 2, info.Height / 2 - circleRadius, info.Width / 2 + info.Height / 50, info.Height / 2 - circleRadius - info.Height / 50, paint);
            canvas.DrawLine(info.Width / 2, info.Height / 2 - circleRadius, info.Width / 2 - info.Height / 50, info.Height / 2 - circleRadius - info.Height / 50, paint);
            canvas.DrawCircle(info.Width / 2, info.Height / 2, circleRadius, paint);
            for (int i = 0; i < (6 * numberOfParticipants); i++)
            {
                canvas.RotateDegrees(360 / (12 * numberOfParticipants), info.Width / 2, info.Height / 2);
                canvas.DrawLine(info.Width / 2, info.Height / 2, info.Width / 2 + circleRadius, info.Height / 2, paint);
                canvas.RotateDegrees(360 / (12 * numberOfParticipants), info.Width / 2, info.Height / 2);
                canvas.DrawText(participants[i % numberOfParticipants], info.Width / 2 + circleRadius / 3, info.Height / 2, paint);
            }


            if (spinCircle)
            {
                canvas.Clear();
                canvas.DrawLine(info.Width / 2, info.Height / 2 - circleRadius, info.Width / 2 + info.Height / 50, info.Height / 2 - circleRadius - info.Height / 50, paint);
                canvas.DrawLine(info.Width / 2, info.Height / 2 - circleRadius, info.Width / 2 - info.Height / 50, info.Height / 2 - circleRadius - info.Height / 50, paint);
                canvas.RotateDegrees((float)spinAmount, info.Width / 2, info.Height / 2);
                canvas.DrawCircle(info.Width / 2, info.Height / 2, circleRadius, paint);
                for (int i = 0; i < (6* numberOfParticipants); i++)
                {
                    canvas.RotateDegrees(360 / (12 * numberOfParticipants), info.Width / 2, info.Height / 2);
                    canvas.DrawLine(info.Width / 2, info.Height / 2, info.Width / 2 + circleRadius, info.Height / 2, paint);
                    canvas.RotateDegrees(360 / (12 * numberOfParticipants), info.Width / 2, info.Height / 2);
                    canvas.DrawText(participants[i % numberOfParticipants], info.Width / 2 + circleRadius / 3, info.Height / 2, paint);
                }
                
                
            }
        }

        async Task Animation(object sender)
        {
            for (int i = 1; i <= 270; i++)
            {
                
                spinAmount = (1 + Math.Log10(0.1+(Double)i / 300)) * totalSpinAmount ;
                (sender as SKCanvasView).InvalidateSurface();
                await Task.Delay(TimeSpan.FromSeconds(1.0 / 90));
            }
            
        }
    }
}