**The University of Melbourne**
**GROUP 69**
# COMP30019 – G69 Toon Racers - README

## Table of Contents
* [Where to Locate Code](#Where-to-Locate-Code)
* [Team Members and Contributions](#Team-Members-and-Contributions)
* [Explanation of the Game](#Explanation-of-the-Game)
* [How to Use the Game](#How-to-Use-the-Game)
* [Objects and Entities](#Objects-and-Entities)
* [Graphics Pipeline and Camera Motion](#Graphics-Pipeline-and-Camera-Motion)
* [User Interface](#User-Interface)
* [Procedural Generation](#Procedural-Generation)
* [Custom Shaders](#Custom-Shaders)
* [Particle System](#Particle-System)
* [Evaluations](#Evaluations)
* [References](#References)
* [Technologies](#Technologies)

## Where to Locate Code

### Water Shader Location
##### Assets/Shaders/WaterShader2
* The water shader is visible in the Time Trial between corner 2 and 3. Also within the Coin Game any height lower the y = 5.

### Cel Shader Location
##### Assets/Shaders/CelShader
* The CelShader is visible on every object in our game except for Water.

### Particle System Location
##### Assets/Particle
* The particle system consists of the three different effects that happen when the car is driving, turning, and boosting.

### Procedural Generation Location
##### Assets/Scripts/CoinGame
* Procedural generation is used only in the Coin Game and is used to provide random worlds and is also responsible for the spawning of the trees and coins.
* Can be reset within the Coin Game using the R key to see how the seed can change.

*Note: 
The Coin Game was included solely to be marked for the procedural generation portion of the assignment. The Time Trials version of our game contains the bulk of our assessable code (everything except for procedural generation). Our decision to implement procedural generation to the Coin Game rather than the Time Trials was motivated by wanting to maintain consistency. By providing the same racetrack to players in our Time Trials, they are able to become familiar with the track, improve their skills, and work towards achieving a personal best time. Whereas, the goals of the Coin Game do not require the same terrain each play.

## Team Members and Contributions

| Name           |            Task            | State   |
| :---:          |            :--:            | :---:   |
| Paul Hutchins  | Map Creation               |  Complete   |
| Paul Hutchins  | Car Movement               |  Complete   |
| Paul Hutchins  | Camera                     |  Complete   |
| Jasper Robbins | Main Menu                  |  Complete   |
| Jasper Robbins | Pause Menu                 |  Complete   |
| Kian Dsouza    | Checkpoints                |  Complete   |
| Jasper Robbins | Checkpoint Tracking        |  Complete   |
| Jade Siang     | Countdown                  |  Complete   |
| Jade Siang     | Timer                      |  Complete   |
| Jasper Robbins | Game Music                 |  Complete   |
| Kian Dsouza    | Procedural Coin Game       |  Complete   |
| Kian Dsouza    | Coin Counter               |  Complete   |
| Kian Dsouza    | Countdown timer            |  Complete   |
| Paul Hutchins  | Particles                  |  Complete   |
| Paul Hutchins  | Shader 1                   |  Complete   |
| Paul Hutchins  | Shader 2                   |  Complete   |
| Jasper Robbins | Persistent Slider Settings |  Complete   |
| Jasper Robbins | Persistent High Score      |  Complete   |
| Paul Hutchins  | Game Video                 |Submitted|
| Jade Siang     | Querying Method           |  Complete   |
| Kian Dsouza    | Observational Method      |  Complete   |
| Paul Hutchins  | Turn Acceleration          |  Complete   |
| Paul Hutchins  | Car Boost                  |  Complete   |
| Jasper Robbins | Checkpoint Splits          |  Complete   |
| Everyone       | Write Up                   |Complete|

## Explanation of the Game
Toon Racers is a racecar game. In the Time Trials version of the game, the player controls a racecar with the aim of completing a lap of the racetrack in the shortest amount of time possible. 

The player is also required to pass through all of the checkpoints on the track in chronological order to successfully complete the lap and stop the timer.

Alternatively, there is also a Coin Game version of the game, where the player aims to collect as many randomly generated coins as possible within the time limit. The game ends when the countdown timer reaches zero and the player is out of time.

## How to Use the Game
The game begins with a main menu from which the gameplay can be accessed. This includes a Play and Quit button, Instructions which details the two different modes (Time Trials, Coin Game), how to play the game, an Options button to adjust the camera and volume settings, as well as a Back button to return to the previous menu.

The menu is easy to read and use, and can be navigated with mouse clicks. All sub-menus can be exited using the escape key for ease of use.

The player is able to control the movement and direction of the racecar using WASD or the arrow keys on the keyboard to move the car in the forward, backward, left, and right directions.

Controls implemented: 
- WASD / arrow keys = direction of racecar 
- R = restart game (Time Trials) / spawn a new procedural terrain (Coin Game)
- F = respawn to most recent checkpoint 
- space bar = boost acceleration 
- escape key = pause / unpause game 

## Objects and Entities

### The Car
The car is implemented as a carMesh rotation locked with a sphere, where all the motion of the car is handled through the sphere which gives the car a nice "arcadey feel".

We initially obtained the directional input using getAxisRaw on "vertical" control inputs.

Also baked into the CarController code, we have a cool-down on car resets which stops the car from moving for a specified amount of time after resetting its position. This works in conjunction with input "space" which resets the cars position to its last checkpoint. 

The car is then moved to the position of the sphere to maintain continuity between the car and the sphere body.

The code then handles horizontal input and goes through an if statement where we only allow the car to turn if its on the ground, as the car should not be able to rotate if its wheels are not in contact with the ground. In the same if statement we check the direction of the car, and reverse the steering input if it is driving backwards.

If the car is not on the ground, it is assumed to be in the air, which we then have a counter to keep track of how long the car has been in air for. If the time in air is over 0.2s, we return the car to horizontal, which again is done to create a more natural drive feel for the player.

The car then shoots a ray from the bottom of the car. If this ray collides with anything within 1 unit, the car body is rotated to stay level with the object it is driving on. This is to stay consistent with hills and slopes the car may drive on.

We also have two levels of drag, one for when the car is in the air and one for when it is on the ground. To make the car feel more realistic, the drag while on the ground is increased, but felt too slow whilst in the air. To contest this, the drag is set to a lower value when the car is in the air in order to mimic the effects of gravity in real life. 

The wheel meshes are also rotated in accordance to the turn input and speed of the car. This allows the car to seem more realistic for the player, rather than having stationary components.

### The Map
For the map, we obtained an artistic prefab with meshes from the unity asset store. We set any objects which we deem as 'ground' in a seperate ground layer. This layer is used to change the car's angle towards the ground and allows us to ensure the car does not interact with other layers like the water and checkpoints.

### Checkpoints

We added a checkpoint functionality to the game where the player would have to pass through all the checkpoints to finish the lap. If the player was to miss some checkpoints or get lost in the track, we have also implemented the ability to respawn from their last valid checkpoint by pressing the F key. 

Checkpoints were implemented as planes and were mainly transparent with slight opacity to indicate that they were a checkpoint. Once a player reached a valid checkpoint in the correct order, it would be marked as completed in the checkpoint counter in the top right of the screen and would turn green.

Checkpoints were made as a prefab, and were managed by a CheckPointManager script which stored the position the car intersected with the last checkpoint.

#### Updates to Car Control After Feedback

After recieving feedback that the car control was too twitchy and hard to pickup, we implemented a couple of changes to aid in making car control easier. We also added a Boost mechanic to accommodate for advanced players and allow them to still enjoy the game.

##### Improving Steering
To make car steering easier for all players and to decrease the learning curve of the game, we reduced the overall speed of the car by 12%, and added an exponential acceleration to the steering to allow quick inputs to be more precise.

Previously the turning input was either -1 or 1, full left or full right, but with our exponential steering control it takes 0.1 seconds of holding down the turn keys to have 100% steering on the car. But at 0.05 seconds the player is only at 25% steering input. This allows players to gain a better control of the car.

##### Adding Boost
With the decrease in speed of the car to aid newer players, we added a Boost mechanic to add another skill for advanced players to master. Now, when the player presses the space bar, the car gains a speed boost of 1.2x its ordinary speed. We also decreased drag, which makes the car move faster, but also makes it harder to turn as it has less grip. This makes the boost mechanic high risk-high reward and is an additional challenge without compromising the accessibility of our game for players with a range of skill levels. 

## Graphics Pipeline and Camera Motion

### Graphics Pipeline
The only modification we made to the graphics pipeline was in our two shaders and in the followCam of the car. Both shaders were vertex shaders that used either vertex normals, or world positions to modify their position or the colour of their respective meshes. More detail on how we modified the vertices is specified below under [shaders](#Custom-Shaders).

We also allow the player to modify their camera settings, which changes how far the camera follows from the car, affecting processes in the Geometry portion of the graphics pipeline. More detail about the [camera](#Camera-Motion) can be found below.

### Camera Motion
Camera motion was achieved mainly by using preset distance targets and then by using Mathf.SmoothDampAngle to ensure smooth camera movement. Otherwise we would've ended up with a jittery effect that was unpleasant to play. Using SmoothDampAngle also allowed us to specify a snap time, so the camera was not always fixed to the car's movement and provided a more natural camera angle for the player.

Below is a snippet of code for the camera height which demonstrates how this was acheived in code: 

```
wantedHeight = cameraTarget.position.y + PlayerPrefs.GetFloat("CameraHeight");
currentHeight = transform.position.y;

currentHeight = Mathf.SmoothDampAngle(currentHeight, wantedHeight, ref heightVelocity, heightDamping, Mathf.Infinity, myTime);
```

We initially obtain the wanted camera height from the player's preference slider in the settings. This allows the player to modify their camera angle within the game.

Then, we find the current height of the camera transform.

Using Mathf.SmoothDampAngle we obtain the next height to set the camera to that is consistent with our heightDamping specification. The other variables just specify the fasted the camera is allowed to move which is set to infinity, and myTime is a float that if the deltaTime = 0, myTime = 0.001 as we were getting errors with division by zero when deltaTime was small.

The same method is used for rotation and distanceToMove, and is all compiled by multiplying the rotation eurler by the distanceToMove vector + height position. The camera is then always set to look directly at the car.

## User Interface
We used a consistent font and text colour throughout our menus for reability. All menus have slightly darkened backgrounds to ensure the text stands out for players. It also ensures they are aware that they have been taken out of the game when pausing or finishing a lap. 

Navigation between menus was assisted by the intuitive escape button navigation out of each menu in addition to the Back button on screen. Controls and Instructions were separated from the sliders section to help organise the menu.

Persistence for music and camera settings in the  sliders were added using PlayerPrefs, storing each setting individually. As the audio was linked to the slider in a submenu, this had to be specifically loaded upon starting the game to fit the music volume to the settings.

The high scores were also stored using PlayerPrefs. Since this is currently designed to be a single player game, difficulty in breaching and modifying the scores is not a priority.

## Procedural Generation
For Toon Racers, we created a Coin Game mode by means of a procedurally generated terrain. The terrain was generated using the Diamond Square Algorithm. This algorithm was preferred to Perlin Noise as it was much faster to generate and it creates hilly terrain with adequate variance. The downside to this algorithm in comparison to Perlin, was that more elements were required to be generated than required because the algorithm uses these elements to average the heights (in a diamond and square pattern).

The algorithm begins with a two-dimensional square array of width and height 2^n + 1. Then the corners were set, with the bottom right fixed at a certain height, which is where the car would be spanned to avoid the car falling into trees and being stuck on top of them, and the other 3 corners (i.e top left, top right, bottom left) were randomly assigned between 0 and the max height. These four corners influence the rest of the rendering. The diamond and square steps are alternately performed recursively where smaller portions of the heightmap were split into squares and their midpoints were updated during the square phase. Similarly, the map was split into diamonds and the center points of the diamonds were updated until all array values were set.

Both these steps work similarly but draw data from different points. The square phase averages four corner points, whereas the diamond phase averages four edge points.

During the square step, points located on the edges of the array only three adjacent values set rather than four. Regardless, their average was calculated using only the three adjacent values. 


## Custom Shaders
### Shader 1 - Water Shader (2 Mark Shader)
The first shader implemented is a basic water shader that builds upon what we learnt in the subject tutorials. This shader can be found around the first couple of corners of the race track and in the code location specified [above](#Where-to-Locate-Code).

This shader uses offset sin waves across the x and z direction both with differing speed to create more natural looking water. A water texture which moves across the surface was also added. Lastly, we coded in our own diffuse lighting effect that had to be done in the same shader path as the moving of the texture otherwise we would obtain two seperate layers of water due to how shader passes are additive and don't build upon one another.

```
vertOut vert(vertIn v)
            {
            
                // Sin Wave Creation
                fixed4 vertWorldPos = mul(unity_ObjectToWorld, v.vertex);
                half offsetvert = 1.5f * vertWorldPos.x * vertWorldPos.x + 0.7f * vertWorldPos.y * vertWorldPos.y;
                half value = _Scale * sin(_Time.x * _Speed + offsetvert * _Frequency);
                v.vertex.y += value;

                // Initialising output vert
                vertOut o;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPosition = vertWorldPos;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // Texture Movement
                o.uv.x += _Speed2.x * _Time.x;
                o.uv.y += _Speed2.y * _Time.x;

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (vertOut i) : SV_Target
            {
                // Diffusion
                fixed3 lightDifference = i.worldPosition - _LightPoint.xyz;
                fixed3 lightDirection = normalize(lightDifference);
                fixed intensity = -0.5 * dot(lightDirection, i.worldNormal) + 0.5;
                fixed4 col = fixed4(intensity * tex2D(_MainTex, i.uv));
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                
                return col;
            }
            ENDCG
```



### Shader 2 - Cel and Outline Shader (5 Mark Shader)
For the second shader in our game, and to match the style of our Toon Racers game, we decided to create a Cel Shader. To work in conjunction with the Cel Shader, we also implemented an outline shader to further develop the toon styling of the game. The shader is combined into one Cel Shader file that runs in two passes, with the Cel Shading in the first pass and the outline shader in the second pass.

#### Cel Portion
For the Cel portion of the shader, it works similarly to a diffuse shader, but instead of directly multiplying the light normal by the surface normal to calculate the intensity, we have an image that has 3 colour values which is used to obtain the colour "intensity" the light should have. So instead of having a linear change in intensity across normals, we now have steps within the value which means the light shading is less smooth and there are jumps between the shading which gives us the Cel effect.

When testing, we realized that the results of purely implementing celShading did not quite fit the style of our game. Therefore, we introduced a combination of Cel Shading and Diffuse Shading along with a saturation level in our final shader.

Below is the main section of code for the Cel Shader:

```
vertOut vert (vertIn v)
            {
                fixed4 vertWorldPos = mul(unity_ObjectToWorld, v.vertex);

                vertOut o;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = vertWorldPos;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

                return o;
            }

fixed4 frag (vertOut i) : SV_Target
            {

                // sample the texture
                fixed3 lightDifference = i.worldPos - _LightPoint.xyz;
                fixed3 lightDirection = normalize(lightDifference);
                fixed4 Colour = fixed4(tex2D(_MainTex, i.uv));
                Colour *= _Brightness;
                
                float2 Tex = i.uv;
                Tex.y = 0.0f;
                Tex.x = 1 - saturate(dot(lightDirection, i.worldNormal));
                float4 CelColour = tex2D(_CelMap, Tex);
               

                return Colour + Colour * CelColour;
            }
            ENDCG
```

The first operation initialises all our needed variables and obtains our worldNormal, Vertexpos, VertexWorldPos, and Texture. The actual shader work occurs in the next portion.

We start out by obtaining the lightDirection from the current vertex. This allows us to measure what intensity of light the object will have at this position. We then obtain the colour of the vertex through the mainTex. Another addition was a brightness modifier as we had issues with the shader being too dark.

The next portion takes the texture colour and turns it into (x,y) values so we can use it in conjunction with our CelMap, which is just an image with different shades of grey to allow us the modify the shading of the object.

Tex.y is set to 0 as it is unused due to us obtaining the colour the CelMap only on the x coordinate. We then dot the light direction and worldNormal to obtain the light intensity angle and saturate fixes any issues with obtaining too high or low of a value as it clamps it between [0..1].

We then use the Tex value to obtain the colour we need on the CelMap. This returns a celColour which is the shade of grey calculated for the light on that particular vertex.

We then multiply the Colour of the object by the CelColour to obtain the final Cel Colour.

Lastly we add the Cel Colour to the object's original colour as otherwise the vertexes on the extremes of the CelMap would be completely white or black. This allows us to have less extremes in our Cel Shading.

#### Outline Portion

The Outline portion of the shader is extremely simple. Initially we had researched into making and edge map with C# and using that to colour all the edges black. However, as we proceeded, it became evident that it was too complex and challenging to implement. 

We had the idea to extend the current vertex and to draw it slightly behind the current face. This gives us the look of a slightly extended black outline without any of the complications we were faced using an edge map. 

We were quite happy with the results of this method and content with the simplicity of our approach and solution. However, if we were not under time constraints, we would have liked to spend more time researching the use of C# scripts in CGShaders and trying to implement it differently. 

The code for the Outline is shown below: 

```
vertOut vert (vertIn i)
			{
                vertOut o;
                o.vertex = UnityObjectToClipPos(float4(i.vertex.xyz+i.normal*_OutlineSize,1));
				return o;
			}
            
			float4 frag (vertOut o) : COLOR
			{
				return float4(_OutlineColor.rgb,0);					
			}          
			ENDCG
```


## Particle System
For the Particle System to stay on theme, we decided to implement tire smoke for the car.

The Particle System is relatively simple and we used almost identical particles for the drift and drive smoke with slight modifications to the size of particles, rate of particle distribution, and materials.

### Design Choices for Particles
As we were constructing the tire smoke, it was deemed neccesary for particles to have an airy feel, along with some form of random noise for the particles. To achieve this we used a sphere shape for emiting particles, with a velocity set behind the car. The particles also had a -0.2 gravity modifier to make them rise slightly.

The velocity and size of the particles were set to decrease over time as the "dust" dissipates, similar to real smoke. Furthermore, the colour of the particles also lightened over time to be consistent with the dissipation. 

Another change to the particles was adding the built in noise graph. This seemed necessary as the particles were deemed linear and unrealisitc during testing. 

Between the two particles, the Drive Smoke was made to be a lighter colour with a dust material applied to it. They were also smaller in overall size and less particles were released. Whereas, the Drift Smoke was darker in colour and had a smoke material applied. 

### Code Functionality
For code funtionality between the two particles, the only input we take is the players "horizontal" input. Where the amount of each smoke is calculated by Drive Smoke = (1-|turnInput|) and Drift Smoke = |turnInput|. The particle present switches depending on when then car is turning or not turning. 

The particles also stop being emitted when the car is not onGround. 

### Updates After Feedback

After receiving feedback from our evaluations, we added another third, Boost Smoke, to the game, in conjunction with the Boost mechanic. This particle works identicle to the other two but with some slight alterations. Moreover, the presence of Boost Smoke is dependent on whether or not the Boost is activated. 

## Evaluations 

All of our evaluations were conducted online and participants consented for their video and audio to be recorded for internal and review purposes. We also typed notes and recorded the participant’s responses, behaviour, and approach during the game, along with anything else that stood out to us as interviewers that we could inquire about further. We also recorded their times for the Time Trials and their scores for the Coin Game. 

The purpose of our evaluation using both querying and observational methods was to measure and determine the usability of game controls, accessibility of menu navigations, and overall visual experience for users. 

The structure of our assessment for both the querying and observational methods were the same, the only difference being the types of questions asked. We asked all participants to rate their gaming competency (beginner, casual/intermediate, advanced) at the start before proceeding to play the game. Beginners were participants with no previous gaming experience, casual/intermediate gamers had limited or some experience, with most participants expressing familiarity with Mario Kart or similar games, and advanced users were confident and had substantial experience in competitive gaming. 

Participants were told that there were two versions of the game, the Time Trials and the Coin Game. They were to successfully complete the Trial Game a minimum of 5 times, followed by some questions, and then play the Coin Game a minimum 5 times, followed again by some questions. We did not prompt, guide, or assist participants on how to navigate or play the game. 

After receiving and implementing the feedback, we reinterviewed the same participants to compare their experiences. The primary goal here was to determine whether or not our changes and amendments achieved its purpose and improved the game controls, visuals, and user experience. 

### Querying Method
We interviewed 5 participants of varying demographics and gaming experiences using an interview styled querying technique. Our decision to conduct one-on-one verbal interviews was because we only needed to reach a limited number of participants (minimum 5). Therefore, it was more beneficial for us to conduct longer but more in-depth interviews with a mix of scalar, multiple choice, and open-ended questions. 

We interviewed 5 participants: 
- 1 advanced male gamer 
- 1 casual/intermediate male gamer 
- 2 casual/intermediate female gamers 
- 1 beginner female gamer 

Some of the fixed questions we asked all participants included: 
- Closed Questions: 
    - What is your competency as a gamer? (beginner, casual/intermediate, advanced) 
    - How easy was the racetrack on a scale of 1-10*? 
    - How visible were the menu and text within the game on a scale of 1-10*? 
    - How easy to use and manage were the controls on a scale of 1-10*? 
    - Overall cohesiveness of the game (in terms of structure, design, theme, visuals etc.) on a scale of 1-10*? 
    
    *Note for scalar questions: 1 = very difficult/non-visible/non-cohesive, 10 = very easy/visible/cohesive 
    
- Open-Ended Questions: 
    - What did you like or enjoy about the game? 
    - Was there anything you found difficult about the game? 
    - Is there anything you would improve or change about the game? 

In addition to the fixed questions provided above, other open-ended questions were asked to individual participants subject to their response. Examples of this might include asking them to explain why they gave the particular score they did, or to elaborate on their response or suggestion to provide more insight into whether or not their feedback is something worth investigating to benefit our general/target audience. 

### Observational Method
We interviewed 5 different participants of varying demographics and gaming experiences using a combination of thinking aloud and post-walkthrough observational techniques. We decided this was the best approach as it allowed us to analyse their thoughts during the game, as well as question and gain insights into their performance afterwards. Whereas, a cooperative evaluation could be quite distracting, particularly given the timed nature of our game in both the Time Trials and Coin Game. 

We interviewed 5 participants: 
- 1 advanced male gamer 
- 1 casual/intermediate male gamer 
- 1 casual/intermediate female gamers 
- 1 beginner male gamer 
- 1 beginner female gamer 

Participants were encouraged to vocalize their thinking and express their thoughts on the controls, visuals, track difficulty, what they liked, challenges etc. After they finished playing, the questions we asked participants were mainly based around their game play and specific events we observed. 
However, these questions can be generalised to include:
- What are you doing at this particular moment? 
- What were you thinking when you did this? 
- Why did you choose to do this? / Did you think about doing this instead? 
- We noticed you did this, why did you decide to do this? 
- You mentioned this when you were playing, can you please elaborate? 

### Feedback and Implementations 
Following both evaluation methods, there were a few areas of common feedback. 

Originally the writing on the main menu, timer, checkpoint counter, countdown, and other miscellaneous writing in the game stood alone as solid blocks of text. Although it was readable, the feedback we received indicated that it was sometimes slightly hard to do so depending on the background. This was a valid area for improvement, so we decided to add shaders behind the text to contrast the background and make the writing more visible. 

Another comment mentioned by all of our participants was the sensitivity of our left and right turning keys. To address this issue and decrease the sensitivity of our keys, we implemented an exponential acceleration on the turning rate to make it less “twitchy”. We also reduced the speed of the car to accommodate less confident players and allow for easier steering. In doing so, this opened up the opportunity for us to introduce a Boost function which would increase the speed of the car but decrease drag, resulting in harder steering (refer to [Adding Boost](#Adding-Boost) for more information). This means that advanced players are able to challenge and upskill themselves, whilst also including another layer of competitiveness within the game. The Boost mechanism was met with a lot of positive feedback during our second round of interviews. 

Something else that was mentioned by one of the advanced gamers in their feedback was to implement a deceleration mechanism to our car, which would allow the car to slowly roll to a stop after removing your finger from the forward key, rather than stopping immediately. Despite being a minor detail, we incorporated this into our improvements as we believed it to be a valid consideration and more accurately mimicked a car, hence providing players with a more realistic experience. 

During our initial observational evaluations, beginner players especially expressed a bit of frustration and were seen to drive off the track bounds and into nearby trees or getting stuck. However, they commented on the advantages of being able to respawn to the last checkpoint in order to “reset” and “gain a fresh start”. Another addition we considered but did not end up including were boundaries or walls around the track to help guide players and prevent them from driving into the surrounding trees. Our decision against this was due to wanting to retain a sense of challenge and competitiveness for players to strategically make shortcuts to beat their highscore, especially accommodating for more advanced gamers. 

It was shown in all of our participants that their first three laps were their slowest times and ranged from over 1 minute to 4 minutes, with advanced gamers having slightly faster times than casual/intermediate and beginner players. There was definitely an observed learning curve in terms of figuring out the best strategy and technique to steer the racecar around the racetrack to achieve the fastest time. We tried to minimize this learning curve through reducing the forward speed of the racecar and sensitivity of the turning keys, however we believe this to be a natural process for players regardless of skill level, as by the fifth lap, we noticed the participant’s times to have decreased significantly to be under 1 minute or just slightly over 1 minute. Further improvements in our participant’s skills and lap times were observed in our follow-up evaluations. 

A requirement of our game to successfully complete a lap of the track is to pass through all of the checkpoints in consecutive order. We included a checkpoint counter in the top left corner of the screen so players can keep track, however beginner and casual/intermediate gamers found it difficult to concentrate on playing whilst also monitoring the counter. To make this visually easier to identify for players, we chose to implement a colour change in the checkpoints from grey to green when successfully passed through. Another suggestion we considered was a “ding” sound to indicate the checkpoint had been hit, but our decision against this was due to it potentially distracting the player or blending into the background music. 

A surprising observation we made during our evaluation was that participants immediately pressed the Play button and started the game without exploring the other menu options or reading the instructions or controls. However, most participants went back to the main menu to read the instructions and controls. 

There was positive feedback for the cohesiveness of the game in terms of the overall structure, design, theme, and visuals, with an average score of 9.6 out of 10 (1 = very non-cohesive, 10 = very cohesive). The deduction in score was credited to the lack of readability of some of the text as mentioned previously. 

The overall difficulty of the racetrack had an average score of 5.6 out of 10 (1 = very difficult, 10 = very easy). Players who identified as beginners were more likely to perceive the track as difficult in comparison to casual/intermediate and advanced gamers. This was understandable and when questioned further, beginner players attributed their response to their skills and comfortability as a gamer, but still expressed content with the general difficulty of the track. We decided to stay with the current track as it incorporated a good balance between easy straights and harder sharp turns, accommodating for a range of skill levels whilst still providing a challenging and engaging experience for players. 

## References

### Unity Assets

SkyBox - https://assetstore.unity.com/packages/vfx/shaders/free-skybox-extended-shader-107400
Terrain - https://assetstore.unity.com/packages/3d/environments/roadways/lowpoly-terrain-track-165394
Car Model - https://assetstore.unity.com/packages/3d/vehicles/land/turbo-cartoon-racing-cars-200116


## Technologies
Project is created with:
* Unity 2021.1.13f1
* Ipsum version: 2.33
* Ament library version: 999
