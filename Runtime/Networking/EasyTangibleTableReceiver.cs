using extOSC;
//using GAG.EasyUIConsole;
using System.Collections.Generic;
using UnityEngine;

namespace GAG.EasyTangibleTable
{
    // Need to be installed extOSC package from the Unity Asset Store to work properly.
    public class EasyTangibleTableReceiver : MonoBehaviour
    {
        public int port = 3333;
        OSCReceiver _receiver;
        readonly HashSet<int> _activeTags = new HashSet<int>();
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _receiver = gameObject.AddComponent<OSCReceiver>();
            _receiver.LocalPort = port;

            _receiver.Bind("/tuio/2Dcur", OnTuioCursorMessage); // Cursor data
            // TUIO object data messages come on /tuio/2Dobj
            _receiver.Bind("/tuio/2Dobj", OnTuioObjectMessage); //Tag data

            _receiver.Bind("/tuio/2Dblob", (message) =>
            {
                // Handle TUIO blob messages if needed
                //print(message.Values.)
            });   
        }

        void OnTuioCursorMessage(OSCMessage message)
        {
            if (message.Values.Count < 1) return;

            var commandType = message.Values[0].StringValue;

            if (commandType == "set")
            {
                if (message.Values.Count >= 6)
                {
                    int sessionID = message.Values[1].IntValue;
                    float xPos = message.Values[2].FloatValue;
                    float yPos = message.Values[3].FloatValue;
                    float xVel = message.Values[4].FloatValue;
                    float yVel = message.Values[5].FloatValue;

                    //EasyUIConsoleManager.Instance.EasyLog($"Touch Detected: SessionID={sessionID}, Pos=({xPos:F2}, {yPos:F2})");
                }
            }
            else if (commandType == "alive")
            {
                // List of currently active touch sessionIDs
            }
            else if (commandType == "fseq")
            {
                // Sync/frame info
            }
        }
        
        void OnTuioObjectMessage(OSCMessage message)
        {
            if (message.Values.Count < 1) return;

            var commandType = message.Values[0].StringValue;

            if (commandType == "set")
            {
                if (message.Values.Count < 11) return; // CAREFUL ABOUT OTHER TUIO Counters This is for Displex Tangible Table

                EasyTangibleTagModel easyTangibleTag = new EasyTangibleTagModel();

                easyTangibleTag.SessionID = message.Values[1].IntValue;
                easyTangibleTag.FiducialID = message.Values[2].IntValue;

                float xPos = message.Values[3].FloatValue;
                float yPos = message.Values[4].FloatValue;
                float remappedX = Mathf.Lerp(1f, 0f, xPos);
                float remappedY = Mathf.Lerp(1f, 0f, yPos);
                easyTangibleTag.XPos = remappedX;
                easyTangibleTag.YPos = remappedY;

                easyTangibleTag.Angle = message.Values[5].FloatValue;
                easyTangibleTag.VelocityX = message.Values[6].FloatValue;
                easyTangibleTag.VelocityY = message.Values[7].FloatValue;
                easyTangibleTag.RotationSpeed = message.Values[8].FloatValue;
                easyTangibleTag.MotionAccel = message.Values[9].FloatValue;
                easyTangibleTag.RotationAccel = message.Values[10].FloatValue;

                _activeTags.Add(easyTangibleTag.SessionID);
                
                //EasyUIConsoleManager.Instance.EasyLog($"Tag Detected: SessionID: {easyTangibleTag.SessionID}, FiducialID: {easyTangibleTag.FiducialID}, Pos = ({easyTangibleTag.XPos.ToString("F2")}, {easyTangibleTag.YPos.ToString("F2")}, Angle: {easyTangibleTag.Degree.ToString("F2")})");
                //print(" Tag Position: (" + easyTangibleTag.XPos.ToString("F2") + ", " + easyTangibleTag.YPos.ToString("F2") + ") Angle: " + easyTangibleTag.Degree.ToString("F2"));
                EasyTangibleTagEvents.RaiseTagPlaced(easyTangibleTag);
            }

            else if (commandType == "alive")
            {
                // From index 1 to end: each value is a session ID (int)
                List<int> activeSessionIDs = new List<int>();

                for (int i = 1; i < message.Values.Count; i++)
                {
                    activeSessionIDs.Add(message.Values[i].IntValue);
                }

                EasyTangibleTagEvents.RaiseTagAlived(activeSessionIDs);

                DetectRemovedTags(activeSessionIDs);
                //string ids = string.Join(", ", activeSessionIDs);
                //EasyUIConsoleManager.Instance.EasyLog("Alive sessions: " + ids);
            }
            else if (commandType == "fseq")
            {
                if (message.Values.Count > 1)
                {
                    int frameNumber = message.Values[1].IntValue;
                    //EasyUIConsoleManager.Instance.EasyHiglight("End of frame #" + frameNumber);
                }
            }
        }
        void DetectRemovedTags(List<int> currentAliveIDs)
        {
            HashSet<int> currentSet = new HashSet<int>(currentAliveIDs);

            foreach (var previousID in _activeTags)
            {
                if (!currentSet.Contains(previousID))
                {
                    EasyTangibleTagEvents.RaiseTagRemoved(previousID);
                }
            }

            _activeTags.Clear();
            _activeTags.UnionWith(currentSet);
        }
    }
}
