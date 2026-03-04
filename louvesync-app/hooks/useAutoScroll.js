import { useRef, useState, useEffect, useCallback } from 'react';

export const useAutoScroll = () => {
  const scrollViewRef = useRef(null);
  const scrollY = useRef(0);
  const animationFrameId = useRef(null);
  
  const contentHeight = useRef(0);
  const viewHeight = useRef(0);

  const [isPlaying, setIsPlaying] = useState(false);
  const [speed, setSpeed] = useState(1);

  // O Motor principal (agora focado apenas em rodar e calcular)
  const loop = useCallback(() => {
    const maxScroll = contentHeight.current - viewHeight.current;
    if (scrollY.current >= maxScroll && maxScroll > 0) {
      setIsPlaying(false);
      return;
    }

    scrollY.current += speed;
    
    if (scrollViewRef.current) {
      scrollViewRef.current.scrollTo({
        y: scrollY.current,
        animated: false,
      });
    }

    animationFrameId.current = requestAnimationFrame(loop);
  }, [speed]); 

  // A MÁGICA DO REACT: O "Vigia" da Ignição
  useEffect(() => {
    if (isPlaying) {
      // Assim que a luz verde do isPlaying acender de verdade, dá a partida
      animationFrameId.current = requestAnimationFrame(loop);
    } else {
      // Se a luz ficar vermelha, freia o motor na hora
      if (animationFrameId.current) cancelAnimationFrame(animationFrameId.current);
    }

    // Função de limpeza (boa prática de segurança para economizar memória)
    return () => {
      if (animationFrameId.current) cancelAnimationFrame(animationFrameId.current);
    };
  }, [isPlaying, loop]);

  // O botão agora SÓ muda o estado. Deixa o useEffect fazer o trabalho sujo.
  const togglePlay = () => {
    setIsPlaying(!isPlaying);
  };

  const handleScrollBeginDrag = () => {
    if (isPlaying) setIsPlaying(false);
  };

  const handleScroll = (event) => {
    scrollY.current = event.nativeEvent.contentOffset.y;
  };

  const handleContentSizeChange = (width, height) => {
    contentHeight.current = height;
  };

  const handleLayout = (event) => {
    viewHeight.current = event.nativeEvent.layout.height;
  };

  return {
    scrollViewRef,
    isPlaying,
    togglePlay,
    speed,
    setSpeed,
    handleScrollBeginDrag,
    handleScroll,
    handleContentSizeChange,
    handleLayout
  };
};