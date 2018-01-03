package shilas.tah_vr;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.speech.RecognitionListener;
import android.speech.RecognizerIntent;
import android.speech.SpeechRecognizer;
import android.os.Bundle;
import android.util.Log;
import android.widget.Toast;

import com.unity3d.player.UnityPlayerNativeActivity;

import java.util.ArrayList;
import java.util.HashMap;

public class MyRecognitionActivity extends UnityPlayerNativeActivity {
    Context mContext = null;
    private SpeechRecognizer sr;
    String str;
    private static final String TAG = "MyRecognitionActivity";
    private final String ACTION_NAME = "startRecognitionAction";
    BroadcastReceiver mBroadcastReceiver;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        Toast.makeText(this, TAG + ": activity overriding is successful", Toast.LENGTH_LONG).show();

        mContext = this;
        str = "";
        registerBroadcastReceiver();
        mBroadcastReceiver = new BroadcastReceiver() {
            @Override
            public void onReceive(Context context, Intent intent) {
                String action = intent.getAction();
                Log.i(action, action);

                if(action.equals(ACTION_NAME)) {
                    SpeechRecognizer sr2;
                    sr2 = SpeechRecognizer.createSpeechRecognizer(mContext);
                    sr2.setRecognitionListener(new Listener());
                    sr2.startListening(new Intent(RecognizerIntent.ACTION_GET_LANGUAGE_DETAILS));
                }
            }
        };
        registerBroadcastReceiver();
    }

    public void startRecognition() {
        Intent mIntent = new Intent(ACTION_NAME);
        sendBroadcast(mIntent);
    }

    public void registerBroadcastReceiver() {
        IntentFilter intentFilter = new IntentFilter();
        intentFilter.addAction(ACTION_NAME);
        registerReceiver(mBroadcastReceiver, intentFilter);
    }

    class Listener implements RecognitionListener {
        private final HashMap<Integer, String> errorMessageTable = new HashMap<Integer, String>(){
            {
                put(SpeechRecognizer.ERROR_AUDIO, "ERROR_AUDIO");
                put(SpeechRecognizer.ERROR_CLIENT, "ERROR_CLIENT");
                put(SpeechRecognizer.ERROR_INSUFFICIENT_PERMISSIONS, "ERROR_INSUFFICIENT_PERMISSIONS");
                put(SpeechRecognizer.ERROR_NETWORK, "ERROR_NETWORK");
                put(SpeechRecognizer.ERROR_NETWORK_TIMEOUT, "ERROR_NETWORK_TIMEOUT");
                put(SpeechRecognizer.ERROR_NO_MATCH, "ERROR_NO_MATCH");
                put(SpeechRecognizer.ERROR_RECOGNIZER_BUSY, "ERROR_RECOGNIZER_BUSY");
                put(SpeechRecognizer.ERROR_SERVER, "ERROR_SERVER");
                put(SpeechRecognizer.ERROR_SPEECH_TIMEOUT, "ERROR_SPEECH_TIMEOUT");
            }
        };

        @Override
        public void onReadyForSpeech(Bundle params) {
            Log.d(TAG, "onReadyForSpeech");
        }

        @Override
        public void onBeginningOfSpeech() {
            Log.d(TAG, "onBeginningOfSpeech");
        }

        @Override
        public void onRmsChanged(float rmsdB) {

        }

        @Override
        public void onBufferReceived(byte[] buffer) {

        }

        @Override
        public void onEndOfSpeech() {
            Log.d(TAG, "onEndOfSpeech");
        }

        @Override
        public void onError(int error) {
            String message = "RECOGNITION ERROR: " +
                    (errorMessageTable.containsKey(error) ?
                            errorMessageTable.get(error) : "UNKNOWN KEY");
            Log.d(TAG, message);
            Toast.makeText(mContext, message, Toast.LENGTH_LONG).show();
            if(error != 0) {
                Intent mIntent = new Intent(ACTION_NAME);
                sendBroadcast(mIntent);
            }
        }

        @Override
        public void onResults(Bundle results) {
            String s = "";
            ArrayList data = results.getStringArrayList(SpeechRecognizer.RESULTS_RECOGNITION);

            String debugMessage = "RECOGNITION RESULT: " + (String)data.get(0);
            Log.d(TAG, debugMessage);

            if(data.size() > 0){
                s = (String)data.get(0);
                str = s;
            }
        }

        @Override
        public void onPartialResults(Bundle partialResults) {
            String s = "";
            ArrayList data = partialResults.getStringArrayList(SpeechRecognizer.RESULTS_RECOGNITION);

            String debugMessage = "PARTIAL RECOGNITION RESULT: " + (String)data.get(0);
            Log.d(TAG, debugMessage);

            if(data.size() > 0){
                s = (String)data.get(0);
                str = s;
            }
        }

        @Override
        public void onEvent(int eventType, Bundle params) {

        }
    }
}
