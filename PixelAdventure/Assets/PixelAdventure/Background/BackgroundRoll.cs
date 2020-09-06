using UnityEngine;

namespace PixelAdventure.Background
{
    public class BackgroundRoll
    {
        private class Node
        {
            public Transform data;
            public Node prev;
            public Node next;

            public Node(Transform data)
            {
                this.data = data;
                this.prev = null;
                this.next = null;
            }
        }

        private Node first = null;
        private Node last = null;

        private float itemHeight = 0f;
        private float borderBottom = 0f;
        private float borderTop = 0f;

        public BackgroundRoll(Transform[] objs)
        {
            // * Create the data structure. (doubly linked list)
            first = new Node(objs[0]);
            last = first;

            for (int i = 1; i < objs.Length; ++i)
            {
                Node node = new Node(objs[i]);
                node.prev = last;
                last.next = node;
                last = node;
            }

            // * Set local fields and set objects position.
            itemHeight = objs[0].GetComponent<SpriteRenderer>().bounds.size.y;

            borderTop = objs[0].position.y;
            foreach (var obj in objs)
                borderTop = obj.position.y > borderTop ? obj.position.y : borderTop;

            Vector3 pos = objs[0].position;
            pos.y = borderTop;

            foreach (var obj in objs)
            {
                obj.position = pos;
                pos.y -= itemHeight;
            }

            borderBottom = objs[objs.Length - 1].position.y - itemHeight;
        }

        public void Roll(float moveAmount, float moveTime)
        {
            // * Advance the 1st position.
            first.data.position = Vector3.Lerp
            (
                a: first.data.position,
                b: first.data.position + new Vector3(0f, moveAmount, 0f),
                t: moveTime
            );

            // * Normalize the others.
            Node node = first.next;
            while (node != null)
            {
                node.data.position = node.prev.data.position + new Vector3(0f, -itemHeight, 0f);
                node = node.next;
            }
            
            // * Replace first and last.
            if (last.data.position.y <= borderBottom)
            {
                last.prev.next = null;
                Node temp = last.prev;
                last.prev = null;

                first.prev = last;
                last.next = first;

                first = last;
                last = temp;

                first.data.position = new Vector3
                (
                    x: first.data.position.x,
                    y: borderTop,
                    z: first.data.position.z
                );
            }

        }

    }
}