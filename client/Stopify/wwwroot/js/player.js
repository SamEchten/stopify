(function () {
    let dotnetRef = null;
    let lastTimeUpdate = 0;

    window.stopifyPlayer = {
        init(ref) {
            dotnetRef = ref;
            const audio = document.getElementById('stopify-audio');
            audio.addEventListener('ended', () => dotnetRef?.invokeMethodAsync('OnAudioEnded'));
            audio.addEventListener('timeupdate', () => {
                const now = Date.now();
                if (dotnetRef && !isNaN(audio.duration) && now - lastTimeUpdate >= 500) {
                    lastTimeUpdate = now;
                    dotnetRef.invokeMethodAsync('OnTimeUpdate', audio.currentTime, audio.duration);
                }
            });
        },

        async play(dataUrl) {
            const audio = document.getElementById('stopify-audio');
            audio.src = dataUrl;
            try {
                await audio.play();
            } catch (e) {
                if (e.name === 'AbortError') return;
                console.error('[player] play error', e);
                dotnetRef?.invokeMethodAsync('OnAudioError', e.message ?? String(e));
            }
        },

        async resume() {
            try { await document.getElementById('stopify-audio').play(); }
            catch (e) { if (e.name !== 'AbortError') console.error('[player] resume error', e); }
        },

        pause() {
            document.getElementById('stopify-audio').pause();
        },

        stop() {
            const audio = document.getElementById('stopify-audio');
            audio.pause();
            audio.src = '';
        },

        seek(time) {
            document.getElementById('stopify-audio').currentTime = time;
        },

        setVolume(volume) {
            document.getElementById('stopify-audio').volume = volume;
        },

        setMuted(muted) {
            document.getElementById('stopify-audio').muted = muted;
        }
    };
})();