using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Models.Dto
{
    /// <summary>
    /// Oluşturulan modellerin apiden gelecek bir response modeline cast edilebileceğini imzalayan interface. Eğer bir model bu imzaya sahip değilse apiden gelecek response içerisinedeki değer bu modele cast edilemez.
    /// </summary>
    public interface _Dto
    {
    }
}
