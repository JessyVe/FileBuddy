package com.fh.joanneum.filebuddymobileclient

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.ListView
import android.widget.SimpleAdapter

class MainActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.list_holder)

        var resultsListView: ListView  = findViewById (R.id.results_listview)

        var nameAddresses: HashMap<String, String> = HashMap()
        nameAddresses.put("Tyga", "343 Rack City Drive");
        nameAddresses.put("Rich Homie Quan", "111 Everything Gold Way");
        nameAddresses.put("Donna", "789 Escort St");
        nameAddresses.put("Bartholomew", "332 Dunkin St");
        nameAddresses.put("Eden", "421 Angelic Blvd");

        var listElements: List<HashMap<String, String>> = ArrayList();
        var adapter: SimpleAdapter = SimpleAdapter(this, listElements, R.layout.list_element,
            String[]{"First Line", "Second Line"},
            int[]{R.id.text1, R.id.text2})


        Iterator it = nameAddresses.entrySet().iterator();
        while (it.hasNext())
        {
            HashMap<String, String> resultsMap = new HashMap<>();
            Map.Entry pair = (Map.Entry)it.next();
            resultsMap.put("First Line", pair.getKey().toString());
            resultsMap.put("Second Line", pair.getValue().toString());
            listItems.add(resultsMap);
        }

        resultsListView.setAdapter(adapter);

    }
    }
}
