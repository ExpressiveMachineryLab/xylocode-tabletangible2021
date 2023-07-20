var JSConnection = {

	JSGetGameQuery: function() {
		var params = new URLSearchParams(window.location.search);
		var gameString = params.get("game");

		if (gameString != null) {
			unityInstance.SendMessage('SL Tool', 'SetTextInput', gameString);
			unityInstance.SendMessage('SL Tool', 'ParseSaveDataString');
		}
	},

	JSGetSIDQuery: function() {
		var params = new URLSearchParams(window.location.search);
		var sidString = params.get("sid");

		if (sidString != null) {

			try{
			unityInstance.SendMessage('Session Manager', 'SetSessionID', sidString + "");
			console.log("Found SID " + sidString);
			}
			catch(e){
				setTimeout(function(){
					unityInstance.SendMessage('Session Manager', 'SetSessionID', sidString + "");
				console.log("Found SID " + sidString);
			}, 5000);
			}
		} else {
			console.log("No SID found");
		}
	},

	JSSendNote: function(noteChannelPair) {
		if (midiDevice == null || midiDevice == undefined) return;
		console.log(noteChannelPair);

		var noteNumber = HEAP32[noteChannelPair >> 2];
		var channel = HEAP32[(noteChannelPair >> 2) + 1];

		var noteLookup = noteNumber + (channel * 128);
		if (channel == null || channel == undefined) channel = 0;

		if (currentMidi[noteLookup] == null || currentMidi[noteLookup] == undefined || currentMidi[noteLookup] == NaN) {
			currentMidi[noteLookup] = 0;
		}
		if (currentMidi[noteLookup] >= 1) { midiDevice.send([128 + channel, noteNumber, 0]); }

		midiDevice.send([144 + channel, noteNumber, 100]);
		currentMidi[noteLookup]++;

		setTimeout(function(){
			if (currentMidi[noteLookup] == 1) {
				midiDevice.send([128 + channel, noteNumber, 0]);
			}
			currentMidi[noteLookup]--;
		}, 500);

		console.log("Playing note " + noteNumber + " on channel " + channel + " on device " + midiDevice.name);
	},

};
mergeInto(LibraryManager.library, JSConnection);
