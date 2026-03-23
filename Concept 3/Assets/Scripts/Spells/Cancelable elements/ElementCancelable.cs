using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ElementCancelable : Element
{
    public List<Element> CancelableElements;

    public bool TryCheckIfCancelable(out Element elementToRemove)
    {
        elementToRemove = SpellManager.Instance.ElementQueue.FirstOrDefault(q => CancelableElements.Contains(q));
        return elementToRemove != null;
    }
}