using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Part3_POE.Services;

public class AudioPlayer
{
   public void Greeting(){
        try{
         SoundPlayer player = new SoundPlayer("Assets/greeting.wav");
         player.Play();
        }catch(Exception ex){
           MessageBox.Show($"Erro Paying the sound:{ex.Message}");

        }
    }
}