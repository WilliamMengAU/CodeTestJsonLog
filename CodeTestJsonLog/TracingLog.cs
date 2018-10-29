using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTestJsonLog
{
    //"time": "2018-08-31T04:05:13+10:00",
    //"app": "svc-app1",
    //"msg": "failed to load user profile for 598e6798-c010-4411-bae8-5effad3388d4, user profile service returned error",
    //"span_id": "e9c0aaef-a560-4585-8b0d-10eefd8c4245",
    //"component": "auth.user.Login()",
    //"level": "debug",
    //"env": "prod",
    //"trace_id": "3394076c-da44-4366-96be-67de338d1c2b"

    class TracingLog : IComparable<TracingLog>
    {
        public DateTime DateTime { get { return DateTime.Parse(time); } }
        public string time { get; set; }
        public string app { get; set; }
        public string msg { get; set; }
        public string component { get; set; }
        public string trace_id { get; set; }

        public void SetFieldByStringArr(string[] lineArr)
        {
            switch (lineArr[0])
            {
                case "time":
                    time = lineArr[1];
                    break;
                case "app":
                    app = lineArr[1];
                    break;
                case "msg":
                    msg = lineArr[1];
                    break;
                case "component":
                    component = lineArr[1];
                    break;
                case "trace_id":
                    trace_id = lineArr[1];
                    break;
                default:
                    break;
            }
        }

        public int CompareTo(TracingLog other)
        {
            return DateTime.CompareTo(other.DateTime);
        }
    }

    class TracingLogLinkNode : IComparable<TracingLogLinkNode>
    {
        public TracingLog TracingLog = new TracingLog();
        public TracingLogLinkNode Next;

        public TracingLogLinkNode()
        {
        }

        public TracingLogLinkNode(TracingLog Item)
        {
            TracingLog = Item;
        }

        public void AddNode(TracingLogLinkNode Node)
        {
            if (Next == null)
            {
                Next = Node;
            }
            else if (Node.CompareTo(Next) < 0)
            {
                Node.Next = Next;
                Next = Node;
            }
            else
            {
                Next.AddNode(Node);
            }
        }

        public int CompareTo(TracingLogLinkNode other)
        {
            return TracingLog.CompareTo(other.TracingLog);
        }
    }

    class TracingLogDict
    {
        public Dictionary<string, TracingLogLinkNode> LogDict = new Dictionary<string, TracingLogLinkNode>();

        public void AddToDict(TracingLog Item)
        {
            TracingLogLinkNode LinkedNode = new TracingLogLinkNode(Item);

            if (LogDict.ContainsKey(Item.trace_id))
            {
                TracingLogLinkNode DictNode = LogDict[Item.trace_id];

                if (Item.DateTime < DictNode.TracingLog.DateTime)
                {
                    LinkedNode.Next = DictNode;
                    LogDict[Item.trace_id] = LinkedNode;
                }
                else
                {
                    DictNode.AddNode(LinkedNode);
                }
            }
            else
            {
                LogDict[Item.trace_id] = LinkedNode;
            }
        }

    }

}
