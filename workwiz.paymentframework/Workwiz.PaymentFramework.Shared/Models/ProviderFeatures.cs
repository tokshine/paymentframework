using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workwiz.PaymentFramework.Shared.Models
{
    [Flags]
    public enum ProviderFeatures
    {
        None = 0,
        SaveCardholder = 1 << 0,
        CheckAuthorization = 1 << 1,
        UpdateCardholder = 1 << 2,
        MediatedModeGui = 1 << 3,
        SaveCard = 1 << 4,
        DeleteSavedCard = 1 << 5
    }
}
