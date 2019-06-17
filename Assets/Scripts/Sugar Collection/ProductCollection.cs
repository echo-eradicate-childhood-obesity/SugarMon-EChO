using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
/// <summary>
/// * This class is used to record information about scanned foods
/// </summary>
public class ProductCollection 
{
    public List<ProductInfo> products = new List<ProductInfo>();
    private List<ProductInfo> curDic = new List<ProductInfo>();
    public List<ProductInfo> CurDic { get { return curDic; } set { curDic = value; } }

    /// <summary>
    /// * Add products to the collection
    /// * When products is not exist or null, create one. Not need as it is created when declare
    /// </summary>
    /// <param name="name">name of product</param>
    /// <param name="pos">where product is scanned</param>
    /// <param name="type">whether or not it has sugar</param>
    public void AddProduct(string name, string UPC, string pos, string sugars)
    {
        if (products == null) products = new List<ProductInfo>();
        var prod = new ProductInfo(name, UPC, pos, DateTime.Now, sugars);
        products.Add(prod);

    }
    /// <summary>
    /// * Removes a product from the collection
    /// </summary>
    /// <param name="pi">Product to remove</param>
    public void RemoveProduct(ProductInfo pi) {
        products.Remove(pi);
    }
    public void ResetCurDic(Category cate) {
        curDic = new List<ProductInfo>();
        if (cate == Category.all) {
            curDic = products;
        }
        else {
            int count = 0;
            foreach (ProductInfo pi in products) {
                if (pi.Type == cate) curDic.Add(pi);
            }
        }
    }

    /*    /// <summary>
        /// * Get Count of products in each category
        /// </summary>
        /// <param name="cate">the Category want to count</param>
        /// <returns></returns>
        public int GetCount(List<Category> cates)
        {
            if (cates.Count > 0)
            {
                var output = 0;
                for (int i = 0; i < cates.Count; i++)
                {
                    output += GetCount(cates[i]);
                }
                return output;
            }
            else { return GetCount(); }
        }*/
    /// <summary>
    /// * Get count of product of that category
    /// * When the not parameter is passed, then use default uncate and return all product count
    /// </summary>
    /// <param name="cate">One specific Category</param>
    /// <returns></returns>
    public int GetCount(Category cate)
    {
        if (cate == Category.all)
            return products.Count;
        int count = 0;
        foreach (ProductInfo pi in products) {
            if (pi.Type == cate) count++;
        }
        return count;
    }
    /// <summary>
    /// * Function used in test to clear all products
    /// * Save as Empty File
    /// </summary>
#if UNITY_EDITOR
    internal void Reset()
    {
        products = new List<ProductInfo>();
        PCSave();
    }
#endif
    /// <summary>
    /// using StreamWriter to writer all the products into an file at "Application.persistentDataPath"
    /// </summary>
    public void PCSave()
    {
        using (StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/test.txt"))
        {
            foreach (ProductInfo pi in products)
            {
                // This block of code removes a \n from behind the UPC which is added to some products for an unknown reason
                string upc = pi.UPC;
                if (upc[upc.Length - 1] == '\n')
                    upc = upc.Substring(0, upc.Length - 1); // trims off the unneeded new line character

                writer.WriteLine($@"{pi.Name};{upc/*pi.UPC*/};{pi.Location};{pi.getScanDateTimeAsString()};{pi.Sugars}");
            }
        }
        Debug.Log("Item Saved Successfully");
    }

    /// <summary>
    /// * Test Function to save as binary file
    /// * I think binary can only save primary type
    /// * Cause error when Deserialize
    /// </summary>
    //public void BinarySave()
    //{
    //    FileStream fs = new FileStream(Application.persistentDataPath + "test.dat", FileMode.Append);
    //    BinaryFormatter formatter = new BinaryFormatter();
    //    try
    //    {
    //        formatter.Serialize(fs, products);
    //    }
    //    catch (Exception e) { Debug.Log(e.Message); }
    //    finally { fs.Close(); }
    //}

    //public void BinaryLoader()
    //{
    //    FileStream fs = new FileStream(Application.persistentDataPath + "test.dat", FileMode.Open);
    //    BinaryFormatter formatter = new BinaryFormatter();
    //    try { products = (List<ProductInfo>)formatter.Deserialize(fs); }
    //    catch (Exception e) { Debug.Log(e.Message); }
    //    finally { fs.Close(); }
    //}

    /// <summary>
    /// * Load file as Streamming reader
    /// * split each line into and string array. 
    /// * first[0] element is the product name
    /// * second[1] is location
    /// * third[2] is category
    /// * Add to products
    /// * This can be an void without return if change the call without sign it to an value?
    /// </summary>
    public List<ProductInfo> Load()
    {
        string line = "";
        using (StreamReader reader=new StreamReader(Application.persistentDataPath + "/test.txt"))
        {
            products = new List<ProductInfo>();
            while ((line = reader.ReadLine()) != null)
            {
                var arr = line.Split(';');
                var prod = new ProductInfo(arr[0], arr[1], arr[2], ProductInfo.getScanDateTimeFromString(arr[3]), arr[4]);
                products.Add(prod);
            }
        }
        return products;
    }

    /// <summary>
    /// Return information about the product in products
    /// </summary>
    /// <param name="i">index</param>
    /// <param name="cate">that category</param>
    /// <returns></returns>
    public string PrintInfo(int i, Category cate)
    {
        if (products != null)
        {
            return products[i].Name;
        }
        return "No Product";
    }
    /// <summary>
    /// Return information about the product in products
    /// </summary>
    /// <param name="i">index</param>
    /// <param name="cates">that category</param>
    /// <returns></returns>
    public ProductInfo GetProduct(int i, Category cate)
    {
        if (cate == Category.all)
        {
            return GetProduct(i, products);
        }
        else
        {
            List<ProductInfo> categoryList = new List<ProductInfo>();
            foreach(ProductInfo pi in products) {
                if (pi.Type == cate) categoryList.Add(pi);
            }

            return GetProduct(i, categoryList);
        }
    }

    private ProductInfo GetProduct(int i, List<ProductInfo> prods) {
        if (i > 0 && i < prods.Count) {
            return prods[i];
        }
        string str = "No More Product";
        string upc = "-1";
        string pos = "Unknown Location";
        Func<ProductInfo> returnNoProd = () => {
            return new ProductInfo(str, upc, pos, DateTime.Now, "");
        };
        return returnNoProd();
    }
}
