using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Holds the root of a nested element and runs an OpenElement and
/// CloseElement function on the element and its children using recursion. 
/// The output resembles a tree outline.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ARX_Tree<T>
{
    #region Constructor
    public ARX_Tree(T nestedItem)
    {
        mo_rootElement = nestedItem;
    }
    #endregion

    #region Variables 
    /// <summary>
    /// The parent element and root of the <T> Tree
    /// </summary>
    protected T mo_rootElement;
    #endregion
    
    #region Abstracts
    /// <summary>
    /// Ran once before the first time this element is drawn
    /// </summary>
    public abstract void I_OpenElement(T element);
    

    /// <summary>
    /// Ran when all child elements are resolved.
    /// </summary>
    public abstract T I_CloseElement(T element);

    /// <summary>
    /// Return true if the given eleemnt has ANY children at all.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public abstract bool I_HasChildren(T element);

    /// <summary>
    /// Return true if the given element has ANOTHER sibling of a lower priority.
    /// For example, given five siblings, siblings 0 through 4 will return true. 
    /// Sibling 5 will return false.
    /// </summary>
    /// <param name="elemeent"></param>
    /// <returns></returns>
    public abstract bool I_HasNextSiblingInList(T elemeent);

    /// <summary>
    /// Return true if the given element has a parent element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public abstract bool I_HasParent(T element);
   
    /// <summary>
    /// Return the given element's first child.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public abstract T I_GetFirstChild(T element);


    /// <summary>
    /// Return the given element's parent.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public abstract T I_GetParent(T element);

    /// <summary>
    /// Return the given elements next sibling.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public abstract T I_GetNextSibling(T element);

    /// <summary>
    /// Executes before the opening of the first element in the nest.
    /// </summary>
    /// <param name="element"></param>
    public abstract void I_BeforeFirstElement(T element);

    /// <summary>
    /// Executes after the closing of the last element in the nest.
    /// </summary>
    /// <param name="element"></param>
    public abstract void I_AfterLastElement(T element);
    #endregion

    #region Run
    /// <summary>
    /// Execute the OpenElement and CloseElement functions on all nested elements
    /// </summary>
    /// <param name="element"></param>
    /// <param name="exiting"></param>
    /// <returns></returns>
    public T Run(T element, bool exiting = false)
    {
        if(exiting)
        {
            T parent = I_GetParent(element);

            I_CloseElement(element);

            //If parent is null, exit
            //This is the end of the navigation algorithm
            if (parent == null)
            {
                I_AfterLastElement(element);
                return default(T);
            }

            //If parent has further sibling
            if (I_HasNextSiblingInList(parent))
            {
                //Continue onwards to that sibling
                return Run(I_GetNextSibling(parent));
            }
            //Else if parent has no sibling
            else
            {
                //Continue navigating backwards
                return Run(parent, true);
            }
           
        }
        else
        {
            //If there is no parent, this is the root element, so run the Start
            if (I_HasParent(element) == false)
                I_BeforeFirstElement(element);

            //Open Element
            I_OpenElement(element);
        }
        
        //If Children
        if(I_HasChildren(element))
        {
            //Run Child
            T child = I_GetFirstChild(element);
            return Run(child);
            
        }
        //Else if no children
        else
        {
            //Close self
            I_CloseElement(element);

            //If Siblings
            if (I_HasNextSiblingInList(element))
            {
                //Run Sibling
                return Run(I_GetNextSibling(element));
            }
            //Else if no siblings
            else
            {
                //If has Parent
                if (I_HasParent(element))
                {
                    //Close Parent
                    T parent = I_GetParent(element);

                    //If parent has sibling
                    if (I_HasNextSiblingInList(parent))
                    {
                        //Run parent's sibling
                        I_CloseElement(parent);
                        T parentsSibling = I_GetNextSibling(parent);
                        return Run(parentsSibling);
                    }
                    //Else if parent has no sibling
                    else
                    {
                        //Navigate backwards....somehow
                        return Run(parent, true);
                    }

                }
                //Else if no parent
                else
                {
                    //This section of code is unreachable.
                    //If you are looking for the termination of this algorithm
                    //it is located at the top of this function
                    //within the "exiting" code block
                    return default(T);
                }
            }
        }
        
    }

    #endregion
}
