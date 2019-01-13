//=============================================================================
// System  : ASP.NET Web Control Library
// File    : Collections.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Fri 02/20/04 18:29:03
// Note    : Copyright 2002-2004, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains collection classes used by the controls.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    12/16/2002  EFW  Created the code
//=============================================================================

using System;
using System.Collections;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EWSoftware.Web.Controls
{
	/// <summary>
	/// This is a derived <see cref="System.Collections.ReadOnlyCollectionBase"/>
	/// class that contains a set of selected items from a list control.  The
	/// collection itself cannot be modified, but the items in it can.  It is
	/// also type-safe.
	/// </summary>
    public class SelectedItemsCollection :
        System.Collections.ReadOnlyCollectionBase
    {
        //=====================================================================
        // Base class implementation

        /// <summary>
        /// This version of the constructor takes a
        /// <see cref="ListItemCollection"/> from which it extracts the
        /// selected items.
        /// </summary>
        /// <param name="liColl">The list item collection from which to get
        /// the selected items.</param>
        /// <overloads>There are two overloads for the constructor</overloads>
        public SelectedItemsCollection(ListItemCollection liColl)
        {
            if(liColl != null)
                foreach(ListItem li in liColl)
                    if(li.Selected)
                        this.InnerList.Add(li);
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="siColl">The selected items collection to copy.</param>
        public SelectedItemsCollection(SelectedItemsCollection siColl)
        {
            if(siColl != null)
                foreach(ListItem li in siColl)
                    this.InnerList.Add(li);
        }

        /// <summary>
        /// Check to see if the <see cref="ListItem"/> object is
        /// in the collection.
        /// </summary>
        /// <param name="li">The list item object to find</param>
        /// <returns>True if found, false if not</returns>
        public bool Contains(ListItem li)
        {
            return this.InnerList.Contains(li);
        }

        /// <summary>
        /// Copy the <see cref="ListItem"/> objects to an array
        /// </summary>
        /// <param name="liItems">The array into which the objects are copied</param>
        /// <param name="nIdx">The index at which to start copying</param>
        public void CopyTo(ListItem [] liItems, int nIdx)
        {
            this.InnerList.CopyTo(liItems, nIdx);
        }

        /// <summary>
        /// Get the index of the <see cref="ListItem"/> object
        /// </summary>
        /// <param name="li">The list item object to find</param>
        /// <returns>The index of the found entry</returns>
        public int IndexOf(ListItem li)
        {
            return this.InnerList.IndexOf(li);
        }

        /// <summary>
        /// Collection indexer.
        /// </summary>
        /// <param name="nIdx">The item to get.</param>
        /// <returns>The entry at the requested position</returns>
        public ListItem this[int nIdx]
        {
            get { return (ListItem)this.InnerList[nIdx]; }
        }

        /// <summary>
        /// Convert the selected item values to a comma-separated list
        /// </summary>
        /// <returns>Returns a string containing the values separated by commas</returns>
        public override string ToString()
        {
            StringBuilder strList = new StringBuilder(1024);

            foreach(ListItem li in this)
            {
                if(strList.Length > 0)
                    strList.Append(',');

                strList.Append(li.Value);
            }

            return strList.ToString();
        }

        /// <summary>
        /// Get a type-safe <see cref="SelectedItemsCollection"/> enumerator
        /// </summary>
        /// <returns>A type-safe <b>SelectedItemsCollection</b> enumerator</returns>
        public new virtual SelectedItemsEnumerator GetEnumerator()
        {
            return new SelectedItemsEnumerator(this);
        }
    }

    //=====================================================================
    // A type-safe SelectedItemsCollection enumerator.

    /// <summary>
    /// A type-safe enumerator for the <see cref="SelectedItemsCollection"/> class.
    /// </summary>
    public class SelectedItemsEnumerator : System.Collections.IEnumerator
    {
        // The wrapped enumerator
        private System.Collections.IEnumerator enWrapped;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="liItems">The selected list item collection to enumerate</param>
        public SelectedItemsEnumerator(SelectedItemsCollection liItems)
        {
            this.enWrapped =
                ((System.Collections.ReadOnlyCollectionBase)liItems).GetEnumerator();
        }

        /// <summary>
        /// Type-safe iterator Current method
        /// </summary>
        public ListItem Current
        {
            get { return (ListItem)this.enWrapped.Current; }
        }

        /// <summary>
        /// Type-unsafe IEnumerator.Current
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get { return this.enWrapped.Current; }
        }

        /// <summary>
        /// Move to the next element
        /// </summary>
        /// <returns>Returns true if not at the end, false if it is</returns>
        public bool MoveNext()
        {
            return this.enWrapped.MoveNext();
        }

        /// <summary>
        /// Reset the enumerator to the start
        /// </summary>
        public void Reset()
        {
            this.enWrapped.Reset();
        }
    }

    //=========================================================================

	/// <summary>
	/// This is a derived <see cref="System.Collections.ReadOnlyCollectionBase"/>
	/// class that contains a set of selected item indices from a list control.
	/// The collection itself cannot be modified, but the items in it can.  It
	/// is also type-safe.
	/// </summary>
    public class SelectedIndicesCollection :
        System.Collections.ReadOnlyCollectionBase
    {
        //=====================================================================
        // Base class implementation

        /// <summary>
        /// This version of the constructor takes a
        /// <see cref="ListItemCollection"/> from which it extracts the
        /// selected item indices.
        /// </summary>
        /// <param name="liColl">The list item collection from which to get
        /// the selected item indices.</param>
        /// <overloads>There are two overloads for the constructor</overloads>
        public SelectedIndicesCollection(ListItemCollection liColl)
        {
            if(liColl != null)
                for(int nIdx = 0; nIdx < liColl.Count; nIdx++)
                    if(liColl[nIdx].Selected)
                        this.InnerList.Add(nIdx);
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="siColl">The selected indices collection to copy.</param>
        public SelectedIndicesCollection(SelectedIndicesCollection siColl)
        {
            if(siColl != null)
                foreach(int nIndex in siColl)
                    this.InnerList.Add(nIndex);
        }

        /// <summary>
        /// Check to see if the index is in the collection.
        /// </summary>
        /// <param name="nIndex">The index to find</param>
        /// <returns>True if found, false if not</returns>
        public bool Contains(int nIndex)
        {
            return this.InnerList.Contains(nIndex);
        }

        /// <summary>
        /// Copy the indices to an array
        /// </summary>
        /// <param name="nIndices">The array into which the indices are copied</param>
        /// <param name="nIdx">The index at which to start copying</param>
        public void CopyTo(int [] nIndices, int nIdx)
        {
            this.InnerList.CopyTo(nIndices, nIdx);
        }

        /// <summary>
        /// Get the index in the collection of the specified selected index.
        /// </summary>
        /// <param name="nIndex">The index to find</param>
        /// <returns>The index of the found entry</returns>
        public int IndexOf(int nIndex)
        {
            return this.InnerList.IndexOf(nIndex);
        }

        /// <summary>
        /// Collection indexer.
        /// </summary>
        /// <param name="nIdx">The item to get.</param>
        /// <returns>The entry at the requested position</returns>
        public int this[int nIdx]
        {
            get { return (int)this.InnerList[nIdx]; }
        }

        /// <summary>
        /// Convert the selected indices to a comma-separated list
        /// </summary>
        /// <returns>Returns a string containing the indices separated by commas</returns>
        public override string ToString()
        {
            StringBuilder strList = new StringBuilder(1024);

            foreach(int nIndex in this)
            {
                if(strList.Length > 0)
                    strList.Append(',');

                strList.Append(nIndex);
            }

            return strList.ToString();
        }

        /// <summary>
        /// Get a type-safe <see cref="SelectedIndicesCollection"/> enumerator
        /// </summary>
        /// <returns>A type-safe <b>SelectedItemsCollection</b> enumerator</returns>
        public new virtual SelectedIndicesEnumerator GetEnumerator()
        {
            return new SelectedIndicesEnumerator(this);
        }
    }

    //=====================================================================
    // A type-safe SelectedIndicesCollection enumerator.

    /// <summary>
    /// A type-safe enumerator for the <see cref="SelectedIndicesCollection"/>
    /// class.
    /// </summary>
    public class SelectedIndicesEnumerator : System.Collections.IEnumerator
    {
        // The wrapped enumerator
        private System.Collections.IEnumerator enWrapped;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="siItems">The indices collection to enumerate</param>
        public SelectedIndicesEnumerator(SelectedIndicesCollection siItems)
        {
            this.enWrapped =
                ((System.Collections.ReadOnlyCollectionBase)siItems).GetEnumerator();
        }

        /// <summary>
        /// Type-safe iterator Current method
        /// </summary>
        public int Current
        {
            get { return (int)this.enWrapped.Current; }
        }

        /// <summary>
        /// Type-unsafe IEnumerator.Current
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get { return this.enWrapped.Current; }
        }

        /// <summary>
        /// Move to the next element
        /// </summary>
        /// <returns>Returns true if not at the end, false if it is</returns>
        public bool MoveNext()
        {
            return this.enWrapped.MoveNext();
        }

        /// <summary>
        /// Reset the enumerator to the start
        /// </summary>
        public void Reset()
        {
            this.enWrapped.Reset();
        }
    }
}
