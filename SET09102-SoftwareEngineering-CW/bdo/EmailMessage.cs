using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SET09102_SoftwareEngineering_CW.exceptions;

namespace SET09102_SoftwareEngineering_CW.bdo
{
    public class EmailMessage : Message
    {
        [JsonProperty("subject")] private string _subject;
        [JsonProperty("sportCentreCode")] private string _sportCentreCode;
        [JsonProperty("natureOfIncident")] private string _incidentType;

        public EmailMessage(RawMessage rawMessage) : base(rawMessage)
        {
            var lines = rawMessage.MessageBody.Split(new string[] {Environment.NewLine},
                StringSplitOptions.RemoveEmptyEntries);

            Sender = lines[0];
            Subject = lines[1];

            var i = 3;
            if (_subject.StartsWith("SIR"))
            {
                SportCentreCode = lines[2];
                IncidentType = lines[3];
                IsSir = true;
                i = 5;
            }

            this.MessageText = $"{MessageText}{lines[i - 1]}"; //Add first item without space to avoid extra space
            for (; i < lines.Length; i++)
            {
                this.MessageText = $"{MessageText} {lines[i]}";
            }
        }
        
        public sealed override string Sender
        {
            get => base.Sender;
            set
            {
                if (!IsValidEmail(value))
                {
                    throw new InputException("Sender must be a valid email address.");
                }

                base.Sender = value;
            }
        }
        
        public string Subject
        {
            get => _subject;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InputException("Could not find subject line, please make sure your email has a subject.");
                }

                if (value.Length > 20)
                {
                    throw new InputException("Subject cannot be longer than 20 characters.");
                }

                _subject = value;
            }
        }

        public sealed override string MessageText
        {
            get => base.MessageText;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InputException("The message text cannot be empty.");
                }

                if (value.Length > 1028)
                {
                    throw new InputException("Email message text cannot be longer than 1028 characters.");
                }

                var linkParser = new Regex(
                    @"\b(?:https?://|www\.)\S+\b",
                    RegexOptions.Compiled | RegexOptions.IgnoreCase);
                base.MessageText = linkParser.Replace(value, "<URL Quarantined>");
            }
        }

        public bool IsSir { get; set; } = false;

        public string SportCentreCode
        {
            get => _sportCentreCode;
            set
            {
                if (!IsValidSportCentreCode(value))
                {
                    throw new InputException("Sport Centre Code: \"" + value +
                                             "\" is not valid. Please enter a valid code.");
                }
            }
        }

        public string IncidentType
        {
            get => _incidentType;
            set
            {
                var list = new List<string>
                {
                    "Theft of Properties", "Staff Attack", "Device Damage", "Raid", "Customer Attack", "Staff Abuse",
                    "Bomb Threat", "Terrorism", "Suspicious Incident", "Sport Injury", "Personal Info Leak"
                };
                if (!list.Contains(value))
                {
                    throw new InputException("Nature of Incident \"" + value +
                                             "\" is not valid. Please enter a valid type.");
                }

                _incidentType = value;
            }
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidSportCentreCode(string code)
        {
            return Regex.Match(code, @"\d{2}-\d{3}-\w{2}\b").Success;
        }
    }
}