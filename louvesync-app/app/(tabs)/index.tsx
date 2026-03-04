import React from 'react';
import { StyleSheet, Text, View, ScrollView, TouchableOpacity } from 'react-native';
// Mantenha o caminho com os dois pontos se estiver dentro de app/(tabs)
import { useAutoScroll } from '../../hooks/useAutoScroll'; 

export default function App() {
  const {
    scrollViewRef, isPlaying, togglePlay, speed, setSpeed,
    handleScrollBeginDrag, handleScroll, handleContentSizeChange, handleLayout
  } = useAutoScroll();

  // Simulando exatamente o JSON que sai do nosso SongsController (C#)
  const mockApiData = {
    title: "A Ele a Glória",
    lines: [
      { segments: [{ chord: "G", lyric: "Porque dEle e por Ele" }] },
      { segments: [{ chord: "Em", lyric: "Para Ele são todas as coisas" }] },
      { segments: [{ chord: "", lyric: "" }] }, // Linha em branco
      { segments: [{ chord: "C", lyric: "Porque dEle e por Ele" }] },
      { segments: [{ chord: "D", lyric: "Para Ele são todas as coisas" }] },
      { segments: [{ chord: "", lyric: "" }] },
      { segments: [{ chord: "", lyric: "A " }, { chord: "G", lyric: "Ele a glória" }] },
      { segments: [{ chord: "", lyric: "A " }, { chord: "Em", lyric: "Ele a glória" }] },
      { segments: [{ chord: "", lyric: "A " }, { chord: "C", lyric: "Ele a glória" }] },
      { segments: [{ chord: "D", lyric: "Pra sempre, amém" }] },
      // Repetindo para dar volume de scroll
      { segments: [{ chord: "", lyric: "" }] },
      { segments: [{ chord: "G", lyric: "Porque dEle e por Ele" }] },
      { segments: [{ chord: "Em", lyric: "Para Ele são todas as coisas" }] },
      { segments: [{ chord: "", lyric: "" }] },
      { segments: [{ chord: "C", lyric: "Porque dEle e por Ele" }] },
      { segments: [{ chord: "D", lyric: "Para Ele são todas as coisas" }] },
    ]
  };

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Text style={styles.title}>{mockApiData.title}</Text>
        
        <View style={styles.controls}>
          <TouchableOpacity style={styles.button} onPress={() => setSpeed(Math.max(0.5, speed - 0.5))}>
            <Text style={styles.buttonText}>- Vel</Text>
          </TouchableOpacity>

          <TouchableOpacity style={[styles.button, isPlaying ? styles.buttonPause : styles.buttonPlay]} onPress={togglePlay}>
            <Text style={styles.buttonText}>{isPlaying ? 'PAUSE' : 'PLAY'}</Text>
          </TouchableOpacity>

          <TouchableOpacity style={styles.button} onPress={() => setSpeed(speed + 0.5)}>
            <Text style={styles.buttonText}>+ Vel</Text>
          </TouchableOpacity>
        </View>
      </View>

      <ScrollView
        ref={scrollViewRef}
        style={styles.scrollArea}
        onScrollBeginDrag={handleScrollBeginDrag}
        onScroll={handleScroll}
        onContentSizeChange={handleContentSizeChange}
        onLayout={handleLayout}
        scrollEventThrottle={16}
      >
        <View style={styles.songContainer}>
          {/* Mapeia cada linha da música */}
          {mockApiData.lines.map((line, lineIndex) => (
            <View key={lineIndex} style={styles.lineRow}>
              
              {/* Mapeia cada pedacinho (Segmento) daquela linha */}
              {line.segments.map((segment, segIndex) => (
                <View key={segIndex} style={styles.segmentColumn}>
                  {/* Renderiza a cifra apenas se ela existir */}
                  <Text style={styles.chord}>{segment.chord}</Text>
                  <Text style={styles.lyric}>{segment.lyric}</Text>
                </View>
              ))}

            </View>
          ))}
        </View>
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#121212', paddingTop: 50 },
  header: { padding: 20, backgroundColor: '#1E1E1E', borderBottomWidth: 1, borderColor: '#333' },
  title: { color: '#FFF', fontSize: 24, fontWeight: 'bold', textAlign: 'center', marginBottom: 15 },
  controls: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' },
  button: { backgroundColor: '#333', paddingVertical: 10, paddingHorizontal: 20, borderRadius: 8 },
  buttonPlay: { backgroundColor: '#4CAF50' },
  buttonPause: { backgroundColor: '#F44336' },
  buttonText: { color: '#FFF', fontWeight: 'bold' },
  scrollArea: { flex: 1, padding: 20 },
  
  // --- ESTILOS MÁGICOS DO RENDERIZADOR ---
  songContainer: {
    paddingBottom: 150, // Dá espaço para o auto-scroll passar da última linha
  },
  lineRow: {
    flexDirection: 'row', // Coloca os segmentos um do lado do outro
    flexWrap: 'wrap',     // Se a linha for muito grande, quebra para baixo
    marginBottom: 10,     // Espaço entre uma linha e outra
  },
  segmentColumn: {
    flexDirection: 'column', // Empilha a Cifra em cima e a Letra embaixo
  },
  chord: {
    color: '#4DA8DA',      // Azul clássico de cifras
    fontSize: 18,
    fontWeight: 'bold',
    fontFamily: 'monospace', // O Checklist exigiu!
    minHeight: 22,           // Mantém o espaço mesmo se não tiver cifra nesse pedaço
  },
  lyric: {
    color: '#FFFFFF',
    fontSize: 18,
    fontFamily: 'monospace',
  }
});