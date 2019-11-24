using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SET09102_SoftwareEngineering_CW.exceptions;

namespace SET09102_SoftwareEngineering_CW.bdo
{
    /// <summary>
    /// Email inherits message and adds Subject, SportCentreCode and IncidentType
    /// </summary>
    public class EmailMessage : Message
    {
        private string _subject;
        private string _sportCentreCode;
        private string _incidentType;

        public EmailMessage(RawMessage rawMessage) : base(rawMessage)
        {
            //Split message body into lines
            var lines = rawMessage.MessageBody.Split(new string[] {Environment.NewLine},
                StringSplitOptions.RemoveEmptyEntries);

            Sender = lines[0].Trim(); //First line should be sender
            Subject = lines[1].Trim(); //Second line should be subject

            var i = 2; //Set line index for concatenating message body lines
            if (IsSir) //Subject will set SIR property to true if SIR detected
            {
                SportCentreCode = lines[2].Trim(); //Third line should be sport centre code
                IncidentType = lines[3].Trim(); //Fourth lines should be incident type
                i = 4; //Override line index for concatenating message body lines
            }

            //Add first item without space to avoid extra space
            this.MessageText = string.Join(Environment.NewLine,lines.Skip(i));
        }

        [JsonProperty("sender")]
        public sealed override string Sender
        {
            get => base.Sender;
            set
            {
                //Validate that sender is an email address
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
                //Check that subject exists
                if (string.IsNullOrEmpty(value))
                {
                    throw new InputException("Could not find subject line, please make sure your email has a subject.");
                }

                //Check that subject is no more than 20 characters
                if (value.Length > 20)
                {
                    throw new InputException("Subject cannot be longer than 20 characters.");
                }

                //Check if message is SIR
                if (value.StartsWith("SIR"))
                {
                    //If message is SIR check that subject is valid (i.e. date is correct)
                    if (!IsValidSirSubject(value))
                    {
                        throw new InputException("Message detected as SIR but could not validate date in subject.");
                    }

                    IsSir = true; //Set IsSir to true
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
                //Check that MessageText exists
                if (string.IsNullOrEmpty(value))
                {
                    throw new InputException("The message text cannot be empty.");
                }

                //Check that MessageText is no more than 1028 characters
                if (value.Length > 1028)
                {
                    throw new InputException("Email message text cannot be longer than 1028 characters.");
                }

                base.MessageText = value.Trim();
            }
        }

        //Identifier if message is SIR, ignored in JSON
        [JsonIgnore] public bool IsSir { get; private set; }

        [JsonProperty("sportCentreCode")]
        public string SportCentreCode
        {
            get => _sportCentreCode;
            set
            {
                //Check that SportCentreCode is valid (xx-xxx-xx)
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
                //List of valid incident types
                var list = new List<string>
                {
                    "Theft of Properties", "Staff Attack", "Device Damage", "Raid", "Customer Attack", "Staff Abuse",
                    "Bomb Threat", "Terrorism", "Suspicious Incident", "Sport Injury", "Personal Info Leak"
                };

                //If type is not in list throw error
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
            //Validate email
            return new EmailAddressAttribute().IsValid(email);
        }

        private static bool IsValidSportCentreCode(string code)
        {
            try
            {
                //Check if regex matches code (checks for xx-xxx-xx, numeric only)
                //If regex doesn't match exception will be thrown
                return Regex.Match(code, @"\d{2}-\d{3}-\w{2}\b").Success;
            }
            catch
            {
                //Code is not valid
                return false;
            }
        }

        private static bool IsValidSirSubject(string subject)
        {
            try
            {
                //Check if regex matches subject (checks for dd/mm/yyyy)
                //If regex doesn't match exception will be thrown
                return Regex.Match(subject.Substring(4, 10),
                    @"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$").Success;
            }
            catch
            {
                //Date is invalid
                return false;
            }
        }
    }
}