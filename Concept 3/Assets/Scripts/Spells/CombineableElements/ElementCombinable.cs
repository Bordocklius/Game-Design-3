using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElementCombinable : Element
{
    public List<Element> CombinableElements;
    public List<Element> ResultingElements;

    // Now returns both the resulting combined element and the element in the queue that should be removed.
    public bool TryCheckIfCombinable(out Element combinedElement, out Element elementToRemove)
    {
        elementToRemove = SpellManager.Instance.ElementQueue.FirstOrDefault(q => CombinableElements.Contains(q));
        if (elementToRemove != null)
        {
            combinedElement = ResultingElements[CombinableElements.IndexOf(elementToRemove)];
            return true;
        }
        else
        {
            combinedElement = null;
            return false;
        }
    }
}
