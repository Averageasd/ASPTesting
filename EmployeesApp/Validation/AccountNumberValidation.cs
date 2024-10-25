namespace EmployeesApp.Validation
{
    public class AccountNumberValidation
    {
        private const int startingPartLength = 3;
        private const int middlePartLength = 10;
        private const int lastPartLength = 2;

        public bool IsValid(string accountNumber)
        {
            var firstDelimiter = accountNumber.IndexOf('-');
            var secondDelimiter = accountNumber.LastIndexOf('-');

            // no first delimter so false
            // first delimter and second delimeter have same index
            // which mean acc number does not have enough delimeters
            if (firstDelimiter == -1 || (firstDelimiter == secondDelimiter))
            {
                throw new ArgumentException();
            }

            // extract first part of accountNumber
            var firstPart = accountNumber.Substring(0, firstDelimiter);

            // starting length is not 3 so false
            if (firstPart.Length != startingPartLength)
            {
                return false;
            }


            var tempPart = accountNumber.Remove(0, startingPartLength + 1);

            // extract 2nd part
            // see if it has delimeter
            var middlePart = tempPart.Substring(0, tempPart.IndexOf('-'));
            
            // if middle part length is not 10 then false
            if (middlePart.Length != middlePartLength)
            {
                return false;
            }

            // last part should have length of 2
            // if not, false
            var lastPart = accountNumber.Substring(secondDelimiter + 1);
            if (lastPart.Length != lastPartLength)
            {
                return false;
            }

            return true;
        }
    }
}
