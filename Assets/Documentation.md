# Xylocode Code Documentation

## Rule Boxes
There are a total of 6 rules that players can interact with: **Chords & Notes, Repeat, Music Genres, 
Visual Effects, Emitter Team, and Line Actions.** Emitter Team has not been implemented yet.   

There is the **ButtonList** which are the graphics shown on screen. Clicking on a button will open the
rule tab if it is not opened already, and jumps to the corresponding rule. The ButtonList can be 
accessed via the **Scene > HUD > ButtonList**  

The Code Tab is the scrollview that contains all the rule boxes. The Code Tab can be accessed via 
the **Scene > HUD > CodeTab > Scroll View > Content**  
Each rule has a seperate **SendStateInformation** code as each rule has different options and buttons. 
*Would be great to unify code for each rule.
### Chords & Notes

### Repeat
Repeat will play the sound in succession the number of times selected. 

### Music Genres
Music Genres is a series of three buttons that changes the mode.  

Refer to **[Sound Manager](##Sound-Manager)**, **[Game Manager](##Game-Manager)** and 
**SendStateInformationChord** code.  

Each button, on value changed, will run the corresponding GameManager's SetToSound function. 

### Visual Effects
Produces visual effects. Currently have place holder 2D animations that play whenever activated.

Refere to **SendStateInformation** code. 

### Line Actions
Line Actions are a series of smaller rules. The rule box has a dropdown menu with three options: 
**Instrument, Volume, Destroy**.  
**Instrument** changes the line color to a different color, thus changing the set of sounds.  
**Volume** changes the volume of the sound that line produces.  
**Destroy** destroys the line that has been hit.   

Refer to **SendStateInformationActions** code.

### If, While Buttons
There are three buttons on the left side of each rule. When the **If** Button is pressed, it will obey 
the rule once. After, it will turn off automatically. When the **While** Button is pressed, it will continuously obey the rule. The third 
button is the **On/Off** Button. The top two will only work if the On/Off button is activated.   

Refer to **SelectedElementRepeat** code.

## Line

## Sound Manager
Refer to **SoundManager** code. Attached to GameManager object in scene.
Stores all the sounds produced by the lines.  
- There are currently 3 modes for each color and 5 sounds for each color in a mode.   
    >BlueSounds11 - Sound one of mode one  
    >BlueSounds12 - Sound two of mode one  
    >BlueSounds21 - Sound one of mode two  
    
- Each sound can consist of one or more sounds. Therefore, it is an array of AudioClip.  
  >public AudioClip[] BlueSounds11;  
- Each color has an encompassing 3D array that contains all the information. 3 refers to the 3 modes 
we currently have.
  >private AudioClip[][][] BlueSounds = new AudioClip[3][][];  
  
    ex. Instantiating Mode 1 for Blue Sounds.  
  >BlueSounds[0] = new AudioClip[5][];  
        BlueSounds[0][0] = BlueSounds11;  
        BlueSounds[0][1] = BlueSounds12;  
        BlueSounds[0][2] = BlueSounds13;  
        BlueSounds[0][3] = BlueSounds14;  
        BlueSounds[0][4] = BlueSounds15;  

**AudioClip GetAudio(AudioSource playClip, string lineColor, int pitch, int mode)**

## Game Manager