package com.fh.joanneum.filebuddymobileclient

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import kotlinx.android.synthetic.main.list_holder.*

class MainActivity : AppCompatActivity() {


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.list_holder)

        val exampleList = generateDummyList(500)

        recycler_view.adapter = ListAdapter(exampleList)
        recycler_view.layoutManager = LinearLayoutManager(this)
        recycler_view.setHasFixedSize(true)
    }

    private fun generateDummyList(size: Int): List<ListItem> {
        val list = ArrayList<ListItem>()
        for (i in 0 until size) {
            val item = ListItem("Item $i", "Line 2")
            list += item
        }
        return list
    }

}
