/**
 * Gets random color hex
 */
const GetRandomColor = () => {
  const colors = ['#f56a00', '#7265e6', '#ffbf00', '#00a2ae'];
  const randomColorIndex = Math.floor(Math.random() * 3.9);
  return colors[randomColorIndex];
};

export default GetRandomColor;
