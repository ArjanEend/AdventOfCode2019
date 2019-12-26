

using System.Collections.Generic;

public class QuadTree<T>
{
    private List<T> objects = new List<T>();
    private List<QuadTree<T>> subs = new List<QuadTree<T>>();

    private int x;
    private int y;
    private int w;
    private int h;

    public QuadTree(int x, int y, int width, int height, QuadTree<T> parent = null)
    {
        this.x = x;
        this.y = y;
        this.w = width;
        this.h = height;

        if (width == 1 || height == 1)
            return;

        subs.Add(new QuadTree<T>(x, y, width / 2, height / 2, this));
        subs.Add(new QuadTree<T>(x + width / 2, y, width / 2, height / 2, this)); 
        subs.Add(new QuadTree<T>(x, y + height / 2, width / 2, height / 2, this));      
        subs.Add(new QuadTree<T>(x + width / 2, y + height / 2, width / 2, height / 2, this));
    }

    public void Add(T obj, int x, int y, int w, int h)
    {
        if (this.x < x || this.y < y || x + w > this.x + this.w || y + h > this.y + this.h) 
            return;

        objects.Add(obj);
        for(int i = 0; i < subs.Count; i++)
        {
            subs[i].Add(obj, x, y, w, h);
        }
    }

    public List<T> FetchAt(int x, int y, List<T> objects = null)
    {
        if (objects == null)
            objects = new List<T>();
        for(int i = 0; i < subs.Count; i++)
        {
            subs[i].FetchAt(x, y, objects);
        }

        if (subs.Count == 0)
            objects.AddRange(this.objects);

        return objects;
    }

}