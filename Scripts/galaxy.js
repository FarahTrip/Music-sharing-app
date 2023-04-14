const scene = new THREE.Scene();
const camera = new THREE.PerspectiveCamera(
  75,
  window.innerWidth / window.innerHeight,
  0.1,
  1000
);

const renderer = new THREE.WebGLRenderer();
renderer.setSize(window.innerWidth, window.innerHeight);
document.getElementById("galaxy-animation").appendChild(renderer.domElement);

const geometry = new THREE.SphereGeometry(50, 50, 32);

const textureLoader = new THREE.TextureLoader();
const texture = textureLoader.load("/Content/galaxie2.png");
const material = new THREE.MeshBasicMaterial({
  map: texture,
  side: THREE.BackSide,
});

const sphere = new THREE.Mesh(geometry, material);
scene.add(sphere);

camera.position.z = 100;

function animate() {
  requestAnimationFrame(animate);
  renderer.render(scene, camera);
}

animate();

document.addEventListener("scroll", function () {
  const galaxySection = document.getElementById("galaxy");
  const rect = galaxySection.getBoundingClientRect();
  const screenHeight = window.innerHeight;
  const positionThreshold = screenHeight;
  if (rect.top < positionThreshold && rect.bottom > 0) {
    renderer.setSize(window.innerWidth, window.innerHeight);
  } else {
    renderer.setSize(0, 0);
  }
});

function lerp(start, end, t) {
  return start * (1 - t) + end * t;
}

window.addEventListener("scroll", function () {
  const galaxySection = document.getElementById("galaxy");
  const rect = galaxySection.getBoundingClientRect();
  const screenHeight = window.innerHeight;

  const scrollPercentage = 1 - rect.top / screenHeight;
  const newY = lerp(-100, 0, scrollPercentage);
  camera.position.y = newY;

  const sphereYRotation = window.scrollY / 500;
  sphere.rotation.y = sphereYRotation;
});

const spriteMaterial = new THREE.SpriteMaterial({
  map: new THREE.CanvasTexture(generateSprite()),
  blending: THREE.AdditiveBlending,
});

for (let i = 0; i < 400; i++) {
    const sprite = new THREE.Sprite(spriteMaterial);
    sprite.scale.set(2, 2, 1);
    sprite.position.set(
      Math.random() * window.innerWidth - window.innerWidth / 2,
      Math.random() * 100 - 50,
      Math.random() * 100 - 50
    );
    scene.add(sprite);
  }

function generateSprite() {
  const canvas = document.createElement("canvas");
  canvas.width = 1000;
  canvas.height = 1000;

  const context = canvas.getContext("2d");
  const gradient = context.createRadialGradient(
    canvas.width / 2,
    canvas.height / 2,
    0,
    canvas.width / 2,
    canvas.height / 2,
    canvas.width / 2
  );

  gradient.addColorStop(0, "rgba(135,205,241, 1)");
  gradient.addColorStop(0.2, "rgba(135,205,241, 1)");
  gradient.addColorStop(0.4, "rgba(135,205,241, 1)");
  gradient.addColorStop(1, "rgba(0, 0, 0, 0)");

  context.fillStyle = gradient;
  context.fillRect(0, 0, canvas.width, canvas.height);

  return canvas;
}