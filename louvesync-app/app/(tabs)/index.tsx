import * as KeepAwake from 'expo-keep-awake';
import React, { useEffect, useState } from 'react';
import { StyleSheet, Text, View, ScrollView, TouchableOpacity, ActivityIndicator } from 'react-native';
// Caminho ajustado para a estrutura app/(tabs)
import { useAutoScroll } from '../../hooks/useAutoScroll'; 

export default function App() {
  const {
    scrollViewRef, isPlaying, togglePlay, speed, setSpeed,
    handleScrollBeginDrag, handleScroll, handleContentSizeChange, handleLayout
  } = useAutoScroll();

  // Estados para gerenciar os dados da API
  const [song, setSong] = useState<any>(null);
  const [loading, setLoading] = useState(true);

  // 1. Gerenciamento do Wake Lock (Manter tela acesa)
  useEffect(() => {
    // Ativa o bloqueio ao montar a tela
    KeepAwake.activateKeepAwakeAsync();
    
    // Desativa ao sair para não drenar bateria após o uso
    return () => {
      KeepAwake.deactivateKeepAwake();
    };
  }, []);

  // 2. Busca de dados na API
  const fetchSong = async () => {
    try {
      setLoading(true);
      // Alterado para ID 2 que é a nossa música com texto limpo
      const response = await fetch('http://192.168.1.9:5000/api/Songs/2/play?shift=0');
      const data = await response.json();
      setSong(data);
    } catch (error) {
      console.error("Erro ao conectar com a API:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchSong();
  }, []);

  if (loading) {
    return (
      <View style={[styles.container, styles.center]}>
        <ActivityIndicator size="large" color="#4DA8DA" />
        <Text style={{ color: '#FFF', marginTop: 10 }}>Sincronizando com LouvorApp...</Text>
      </View>
    );
  }

  if (!song) {
    return (
      <View style={[styles.container, styles.center]}>
        <Text style={{ color: '#F44336', marginBottom: 20 }}>Erro ao carregar música.</Text>
        <TouchableOpacity style={styles.button} onPress={fetchSong}>
          <Text style={styles.buttonText}>Tentar Novamente</Text>
        </TouchableOpacity>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Text style={styles.title}>{song.title}</Text>
        <Text style={styles.subtitle}>{song.artist}</Text>
        
        <View style={styles.controls}>
          <TouchableOpacity 
            style={styles.button} 
            onPress={() => setSpeed(Math.max(0.5, speed - 0.5))}
          >
            <Text style={styles.buttonText}>- Vel</Text>
          </TouchableOpacity>

          <TouchableOpacity 
            style={[styles.button, isPlaying ? styles.buttonPause : styles.buttonPlay]} 
            onPress={togglePlay}
          >
            <Text style={styles.buttonText}>{isPlaying ? 'PAUSE' : 'PLAY'}</Text>
          </TouchableOpacity>

          <TouchableOpacity 
            style={styles.button} 
            onPress={() => setSpeed(speed + 0.5)}
          >
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
          {song.lines.map((line: any, lineIndex: number) => (
            <View key={lineIndex} style={styles.lineRow}>
              {line.segments.map((segment: any, segIndex: number) => (
                <View key={segIndex} style={styles.segmentColumn}>
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
  // Fundo Preto Total (#000000) para Modo Palco
  container: { flex: 1, backgroundColor: '#000000', paddingTop: 50 },
  center: { justifyContent: 'center', alignItems: 'center' },
  header: { 
    padding: 20, 
    backgroundColor: '#121212', 
    borderBottomWidth: 1, 
    borderColor: '#333' 
  },
  title: { color: '#FFF', fontSize: 26, fontWeight: 'bold', textAlign: 'center' },
  subtitle: { color: '#888', fontSize: 18, textAlign: 'center', marginBottom: 15 },
  controls: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' },
  
  // Botões com Hitbox aumentada para facilitar o toque rápido
  button: { 
    backgroundColor: '#333', 
    paddingVertical: 12, 
    paddingHorizontal: 25, 
    borderRadius: 8 
  },
  buttonPlay: { backgroundColor: '#4CAF50' },
  buttonPause: { backgroundColor: '#F44336' },
  buttonText: { color: '#FFF', fontWeight: 'bold', fontSize: 16 },
  
  scrollArea: { flex: 1, padding: 20 },
  songContainer: { paddingBottom: 200 },
  lineRow: { flexDirection: 'row', flexWrap: 'wrap', marginBottom: 12 },
  segmentColumn: { flexDirection: 'column' },
  chord: {
    color: '#4DA8DA', // Azul vibrante para alto contraste
    fontSize: 20,
    fontWeight: 'bold',
    fontFamily: 'monospace', // Garantindo alinhamento conforme Card 3
    minHeight: 24,
  },
  lyric: {
    color: '#FFFFFF',
    fontSize: 20,
    fontFamily: 'monospace',
  }
});