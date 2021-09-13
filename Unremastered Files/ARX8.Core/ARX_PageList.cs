using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// Struct does stuff with pages, page numbers and page navigation
/// </summary>
public struct ARX_PageList
{

    public int mn_currentPage;
    public int mn_itemsPerPage;

    public ARX_PageList(int nItemsPerPage)
    {
        mn_currentPage = 0;
        mn_itemsPerPage = nItemsPerPage;
    }

    public void IncrementPage(int nTotalInventory)
    {
        //If incrementing would put the page into an invalid page, return
        if ((mn_currentPage + 1) * mn_itemsPerPage >= nTotalInventory)
            return;

        mn_currentPage++;
    }

    public void DecrementPage()
    {
        if (mn_currentPage == 0)
            return;
        if (mn_itemsPerPage < 0)
            mn_itemsPerPage = 0;
        mn_currentPage--;
    }


    /// <summary>
    /// Returns the indexes of the first and last of the active inventory items.
    /// Eg. If on page 2 of a book with 10 items per page, returns (20, 29) or
    /// if the given inveotory size is 25, returns (20, 25)
    /// </summary>
    /// <param name="nPage"></param>
    /// <param name="nItemsPerPage"></param>
    /// <param name="nInventorySize"></param>
    /// <returns></returns>
    public Vector2Int GetActiveInventoryMinAndMax(int nPage, int nItemsPerPage, int nInventorySize)
    {
        //Validate
        if (nItemsPerPage <= 0)
            nItemsPerPage = 1;
        if (nInventorySize <= 0)
            nInventorySize = 1;


        int nMin = nPage * nItemsPerPage;
        int nMax = nMin + nItemsPerPage - 1;

        //If the page is invalid due to being beyond the inventory size
        if (nMin > nInventorySize)
        {
            //Nothing is active
            return new Vector2Int(0, 0);
        }

        //If there are empty spaces in this page
        if (nMax > nInventorySize)
        {
            nMax = nInventorySize;
        }
        return new Vector2Int(nMin, nMax);
    }

    /// <summary>
    /// Returns the indexes of the first and last of the inactive inventory items
    /// </summary>
    /// <param name="nPage"></param>
    /// <param name="nItemsPerPage"></param>
    /// <param name="nInventorySize"></param>
    /// <returns></returns>
    public Vector2Int GetInactiveInventoryMinAndMax(int nPage, int nItemsPerPage, int nInventorySize)
    {
        //Validate
        if (nItemsPerPage <= 0)
            nItemsPerPage = 1;
        if (nInventorySize <= 0)
            nInventorySize = 1;


        int nMin = nPage * nItemsPerPage;
        int nMax = nMin + nItemsPerPage - 1;

        //If the page is invalid due to being beyond the inventory size
        if (nMin > nInventorySize)
        {
            //Everything is inactive
            return new Vector2Int(nMin, nMax);
        }

        //If there are empty spaces in this page
        if (nMax > nInventorySize)
        {
            nMin = nInventorySize;
        }
        return new Vector2Int(nMin, nMax);
    }


    /// <summary>
    /// Returns the indexes of the first and last of the active inventory items.
    /// Eg. If on page 2 of a book with 10 items per page, returns (20, 29) or
    /// if the given inveotory size is 25, returns (20, 25)
    /// </summary>
    /// <param name="nPage"></param>
    /// <param name="nItemsPerPage"></param>
    /// <param name="nInventorySize"></param>
    /// <returns></returns>
    public Vector2Int GetActiveInventoryMinAndMax(int nInventorySize)
    {
        return GetActiveInventoryMinAndMax(mn_currentPage, mn_itemsPerPage, nInventorySize);
    }

    /// <summary>
    /// Returns the indexes of the first and last of the inactive inventory items
    /// </summary>
    /// <param name="nPage"></param>
    /// <param name="nItemsPerPage"></param>
    /// <param name="nInventorySize"></param>
    /// <returns></returns>
    public Vector2Int GetInactiveInventoryMinAndMax(int nInventorySize)
    {
        return GetInactiveInventoryMinAndMax(mn_currentPage, mn_itemsPerPage, nInventorySize);
    }
    
    public Vector2Int GetActiveSlides(int nPage, int nItemsPerPage, int nInventorySize, int nSlideCount)
    {
        //Given a number of slides, return a vector ranging from 0 to nSlideCount - 1 (minus one) of 
        //the indexes of the slides to be set to active

        //Find the vector of active inventory
        //Find the difference in the active inventory vector
        //Return new Vector2 (0, difference)

        Vector2 vecActiveInventory = GetActiveInventoryMinAndMax(nPage, nItemsPerPage, nInventorySize);
        int nDiff = (int)vecActiveInventory.y - (int)vecActiveInventory.x;

        if (nDiff > nSlideCount - 1)
            nDiff = nSlideCount - 1;

        return new Vector2Int(0, nDiff);
    }

    public Vector2Int GetActiveSlides(int nInventorySize, int nSlideCount)
    {
        return GetActiveSlides(mn_currentPage, mn_itemsPerPage, nInventorySize, nSlideCount);
    }

    public Vector2Int GetInactiveSlides(int nPage, int nItemsPerPage, int nInventorySize, int nSlideCount)
    {
        //Given a number of slides, return a vector ranging from 0 to nSlideCount - 1 (minus one) of 
        //the indexes of the slides to be set to active

        //Find the vector of active inventory
        //Find the difference in the active inventory vector
        //Return new Vector2 (0, difference)

        Vector2Int vecInactiveInventory = GetInactiveInventoryMinAndMax(nPage, nItemsPerPage, nInventorySize);
        Vector2Int vecActiveSlides = GetActiveSlides(nInventorySize, nSlideCount);

        int nDiff = (int)vecInactiveInventory.y - (int)vecInactiveInventory.x;
        nDiff += vecActiveSlides.y;

        if (nDiff > nSlideCount - 1)
            nDiff = nSlideCount - 1;

        return new Vector2Int(vecActiveSlides.y, nDiff);
    }

    public Vector2Int GetInactiveSlides(int nInventorySize, int nSlideCount)
    {
        return GetInactiveSlides(mn_currentPage, mn_itemsPerPage, nInventorySize, nSlideCount);
    }


}

