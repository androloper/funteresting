import speech_recognition as sr
from pydub import AudioSegment

def transcribe_audio_file(filename):
    # Load audio file and convert it to .wav format
    audio = AudioSegment.from_mp3(filename)
    wav_filename = "temp.wav"
    audio.export(wav_filename, format="wav")

    # Load the .wav file using the SpeechRecognition library
    r = sr.Recognizer()
    with sr.AudioFile(wav_filename) as source:
        audio_text = r.record(source)

    # Transcribe audio to text using Google Speech API
    try:
        transcribed_text = r.recognize_google(audio_text)
    except sr.UnknownValueError:
        print("Google Speech API could not understand audio")
        transcribed_text = ""
    except sr.RequestError as e:
        print("Could not request results from Google Speech API: {0}".format(e))
        transcribed_text = ""

    return transcribed_text

if __name__ == '__main__':
    filename = "deneme.mp3"
    text = transcribe_audio_file(filename)
    print(text)
