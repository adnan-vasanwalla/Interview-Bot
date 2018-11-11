package com.example.adnan.interviews;

import android.os.Environment;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;

import java.io.FileInputStream;
import java.io.FileNotFoundException;

public class MainActivity extends AppCompatActivity {

    private Button uploadButton;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        uploadButton = (Button) findViewById(R.id.uploadButton);
        uploadButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                UploadFile();
            }
        });
    }
    public void UploadFile(){
        try {
            String path=Environment.getExternalStorageDirectory().toString()+"/DCIM/Resume.pdf";
            Log.e("Main Activity",path);
            // Set your file path here
            FileInputStream fstrm = new FileInputStream(path);

            // Set your server page url (and the file title/description)
            //HttpFileUpload hfu = new HttpFileUpload("http://localhost:9823/Default.aspx", "my file title","my file description");

            //hfu.Send_Now(fstrm);

        } catch (FileNotFoundException e) {
            Log.e("Main Activity","File not found");
        }
    }
}
