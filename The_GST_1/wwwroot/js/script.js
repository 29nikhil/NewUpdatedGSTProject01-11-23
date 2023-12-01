function setup() {
  TweenMax.set('#archer', {
    scale: 0.85,
    transformOrigin: "50% 50%"
  })
  
  var svg = document.getElementById('ad_01');
  var trig1 = document.getElementById('top_trig');
  var trig2 = document.getElementById('bottom_trig');
  var seeMore = document.getElementById('see-more');
  var learnMore = document.getElementById('learn-more');
  var tl1 = new TimelineMax({repeat:0, paused: true, yoyo: false});
  var tl2 = new TimelineMax({repeat:0, paused: true, yoyo: false});
  var tAnim = 0.3;

  tl1.to('#up-arrow', tAnim, {
    y: -250,
    opacity: 0,
    ease:Back.easeIn.config(1.1),
  })
  .to('#down-arrow', tAnim/2, {
    y: 20,
    opacity: 0,
    ease:Power2.easeOut
  }, '-=' + tAnim)
  .to('#logo', tAnim, {
    y: -250,
    ease:Back.easeIn.config(1.3),
  }, '-=' + tAnim/1.4)
  .to('#top_clip polygon', tAnim, {
    attr:{points:"0,0 300,0 300,0 0,0"},
    ease:Back.easeIn.config(1.3)
  }, '-=' + tAnim)
  .to('#bottom_clip polygon', tAnim, {
    attr:{points:"0,0 300,0 300,600 0,600"},
    ease:Back.easeIn.config(1.3)
  }, '-=' + tAnim)
  .to('#logo-shadow', tAnim, {
    opacity: 0,
    ease:Power3.easeOut,
  }, '-=' + tAnim)
  .set('#logo-back', {
    opacity: 0
  })
  .fromTo('#driveguard', tAnim, {
    y:-15,
    opacity: 0
  },{
    y:0,
    opacity: 1,
    ease:Power2.easeOut
  }, '-=' + tAnim/6)
  .to('#tire', tAnim, {
    x: -100,
    y: -65,
    ease:Power2.easeInOut,
  }, '-=' + tAnim)
  .fromTo('#see-more', tAnim, {
    x: 300,
    opacity: 0
  },{
    x: 0,
    opacity: 1,
    ease:Power2.easeInOut,
  }, '-=' + tAnim)
  .fromTo('#archer-sm', tAnim, {
    x: 150,
    opacity: 0,
    transformOrigin: "50% 50%",
    rotation: "50deg"
  },{
    x: 0,
    opacity: 1,
     transformOrigin: "50% 50%",
    rotation: "0deg",
    ease:Power3.easeInOut,
  }, '-=' + tAnim)
  .to('#dont-let', tAnim, {
    y:50,
    opacity: 0,
    ease:Power3.easeInOut
  }, '-=' + tAnim)
  .fromTo('#drive-up-to', tAnim, {
    opacity: 0,
    y: -10
  },{
    opacity: 1,
    y: 0,
    ease:Power3.easeOut
  }, '-=' + tAnim)
  .fromTo('#yes-even-from', tAnim, {
    opacity: 0,
    y: -10
  },{
    opacity: 1,
    y: 0,
    ease:Power3.easeOut
  }, '-=' + tAnim/1.6);

  tl2.to('#down-arrow', tAnim, {
    y: 280,
    opacity: 0,
    ease:Back.easeIn.config(1.1)
  })
  .to('#up-arrow', tAnim/2, {
    y: -20,
    opacity: 0,
    ease:Power2.easeOut
  }, '-=' + tAnim)
  .to('#dont-let', tAnim, {
    y: 300,
    opacity: 0,
    ease:Power2.easeInOut
  }, '-=' + tAnim)
  .to('#tire', tAnim, {
    y: 300,
    opacity: 0,
    ease:Power2.easeInOut
  }, '-=' + tAnim)
  .to('#logo', tAnim, {
    y: 280,
    ease:Back.easeIn.config(1.3),
  }, '-=' + tAnim/1.4)
  .to('#top_clip polygon', tAnim, {
    attr:{points:"0,0 300,0 300,600 0,600"},
    ease:Back.easeIn.config(1.3)
  }, '-=' + tAnim)
  .to('#bottom_clip polygon', tAnim, {
    attr:{points:"0,600 300,600 300,600 0,600"},
    ease:Back.easeIn.config(1.3)
  }, '-=' + tAnim)
  .to('#logo-shadow', tAnim, {
    opacity: 0,
    ease:Power3.easeOut,
  }, '-=' + tAnim)
  .to('#archer', tAnim, {
    y: 286,
    x: 0,
    scale: 1,
    transformOrigin: "50% 50%",
    ease:Power2.easeInOut,
  }, '-=' + tAnim/1.6)
  .to('#target', tAnim, {
    y: 7,
    x: -60,
    scale: 0.69,
    transformOrigin: "50% 50%",
    ease:Power2.easeInOut,
  }, '-=' + tAnim)
  .fromTo('#get-7', tAnim, {
    x: -60,
    scale: 2,
    opacity: 0,
    transformOrigin: "50% 50%",
  },{
    x: 0,
    scale: 1,
    opacity: 1,
    transformOrigin: "50% 50%",
    ease:Power2.easeInOut,
  }, '-=' + tAnim)
  .fromTo('#learn-more', tAnim, {
    y: -50,
    opacity: 0
  },{
    y: 0,
    opacity: 1,
    ease:Power2.easeInOut,
  }, '-=' + tAnim);

  function reset1() {
    trig1.addEventListener("mousemove", play2, false);
  }
  
  function reset2() {
    trig2.addEventListener("mousemove", play1, false);
  }
  
  tl1.eventCallback("onReverseComplete", reset1);
  tl2.eventCallback("onReverseComplete", reset2);
  
  function play1() {
    tl1.play();
    trig2.removeEventListener("mousemove", play1, false);
    trig1.removeEventListener("mousemove", play2, false);
    svg.addEventListener("mouseleave", rev1, false);
  }
  
  function play2() {
    tl2.play();
    trig2.removeEventListener("mousemove", play1, false);
    trig1.removeEventListener("mousemove", play2, false);
    svg.addEventListener("mouseleave", rev2, false);
  }
  
  function rev1() {
    trig2.addEventListener("mousemove", play1, false);
    svg.removeEventListener("mouseleave", rev1, false);
    tl1.reverse();
  }

  function rev2() {
    trig1.addEventListener("mousemove", play2, false);
    svg.removeEventListener("mouseleave", rev2, false);
    tl2.reverse();
  }

  trig2.addEventListener("mousemove", play1, false);
  trig1.addEventListener("mousemove", play2, false);
  seeMore.addEventListener("click", function(){
    document.location ='https://twitter.com/halvves';
  }, false);
  learnMore.addEventListener("click", function(){
    document.location ='https://twitter.com/halvves';
  }, false);
}

setup();