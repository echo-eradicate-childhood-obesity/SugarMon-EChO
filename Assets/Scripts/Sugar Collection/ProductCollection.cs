using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
/// <summary>
/// * This classed is used to record all information about the green products
/// * Protentially can be expend to all kinds of products by change the save file to direction+
/// </summary>
public class ProductCollection 
{
    public List<ProductInfo> products = new List<ProductInfo>();
    
    public Dictionary<Category, List<ProductInfo>> greenDic=new Dictionary<Category, List<ProductInfo>>();
    private List<ProductInfo> curDic = new List<ProductInfo>();
    public List<ProductInfo> CurDic { get { return curDic; } set { curDic = value; } }

    /// <summary>
    /// * Add products to the collection
    /// * When products is not exist or null, create one. Not need as it is created when declare
    /// </summary>
    /// <param name="name">name of product</param>
    /// <param name="pos">where product is scanned</param>
    public void AddProduct(string name,string pos)
    {
        if (products == null)
        {
            products = new List<ProductInfo>();
        }
        var prod = new ProductInfo(name,pos);
        products.Add(prod);
    }
    /// <summary>
    /// * Get Count of products in each category
    /// </summary>
    /// <param name="cates">the Category want to count</param>
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
    }
    /// <summary>
    /// * Get count of product of that category
    /// * When the not parameter is passed, then use default uncate and return all product count
    /// </summary>
    /// <param name="cate">One specific Category</param>
    /// <returns></returns>
    private int GetCount(Category cate = Category.uncate)
    {
        if (products != null)
        {
            if (cate != Category.uncate)
            {
                return greenDic[cate].Count;
            }
            return this.products.Count;
        }
        return 0;
    }
    /// <summary>
    /// * Function used in test to clear all products
    /// * Save as Empty File
    /// </summary>
#if UNITY_EDITOR
    internal void Reset()
    {
        greenDic = new Dictionary<Category, List<ProductInfo>>();
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
                writer.WriteLine($@"{pi.Name};{pi.Location};{pi.Type}");
            }
        }
        Debug.Log("save done");
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
            if (products == null)
                {
                    products = new List<ProductInfo>();
                }
            while ((line = reader.ReadLine()) != null)
            {
                var arr = line.Split(';');
                var prod = new ProductInfo(arr[0], arr[1],Converter.StringEnumConverter<Category, string>(arr[2]));
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
    public string PrintInfo(int i, Category cate = Category.uncate)
    {
        if (products != null)
        {
            return products[i].PrintInfo();
        }
        return "No Product";
    }
    /// <summary>
    /// Return information about the product in products
    /// </summary>
    /// <param name="i">index</param>
    /// <param name="cates">that category</param>
    /// <returns></returns>
    public ProductInfo GetProduct(int i, List<Category> cates)
    {
        if (cates.Count==0)
        {
            return GetProduct(i);
        }
        else
        {
            return curDic[i];
        }
    }

    private ProductInfo GetProduct(int i)
    {
        if ( i>0&&i < products.Count)
        {
            return products[i];
        }
        string str = "No More Product";
        string pos = "Unknow Location";
        Func<ProductInfo> returnNoProd = () => {
            var pi = new ProductInfo(str,pos);
            return pi;
        };
        return returnNoProd();
    }
}
