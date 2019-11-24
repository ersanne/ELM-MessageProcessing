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
        private string _subject;
        private string _sportCentreCode;
        private string _incidentType;

        public EmailMessage(RawMessage rawMessage) : base(rawMessage)
        {
            var lines = rawMessage.MessageBody.Split(new string[] {Environment.NewLine},
                StringSplitOptions.RemoveEmptyEntries);

            Sender = lines[0].Trim();
            Subject = lines[1].Trim();

            var i = 3;
            if (IsSir)
            {
                SportCentreCode = lines[2].Trim();
                IncidentType = lines[3].Trim();
                IsSir = true;
                i = 5;
            }

            this.MessageText =
                $"{MessageText}{lines[i - 1].Trim()}"; //Add first item without space to avoid extra space
            for (; i < lines.Length; i++)
            {
                MessageText = $"{MessageText} {lines[i].Trim()}";
            }
        }

        [JsonProperty("sender")]
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

        [JsonProperty("subject")]
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

                if (value.StartsWith("SIR"))
                {
                    if (!IsValidSirSubject(value))
                    {
                        throw new InputException("Message detected as SIR but could not validate date in subject.");
                    }

                    IsSir = true;
                }

                _subject = value;
            }
        }

        [JsonProperty("messageText")]
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
                
                base.MessageText = value;
            }
        }

        [JsonIgnore] public bool IsSir { get; set; } = false;

        [JsonProperty("sportCentreCode")]
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

                _sportCentreCode = value;
            }
        }

        [JsonProperty("natureOfIncident")]
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
            try
            {
                return Regex.Match(code, @"\d{2}-\d{3}-\w{2}\b").Success;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidSirSubject(string subject)
        {
            try
            {
                return Regex.Match(subject.Substring(4,10), @"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$").Success;
            }
            catch
            {
                return false;
            }
        }
    }
}