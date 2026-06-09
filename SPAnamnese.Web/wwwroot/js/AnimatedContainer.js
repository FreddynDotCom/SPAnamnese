import { gsap } from 'https://cdn.skypack.dev/gsap';
import { ScrollTrigger } from 'https://cdn.skypack.dev/gsap/ScrollTrigger';

gsap.registerPlugin(ScrollTrigger);

const _instances = {};

function isFixedOrSticky(el) {
    let node = el;
    while (node && node !== document.body) {
        const pos = getComputedStyle(node).position;
        if (pos === 'fixed' || pos === 'sticky') return true;
        node = node.parentElement;
    }
    return false;
}

export function start(opts) {
    const {
        elementId,
        container,
        distance = 100,
        direction = 'vertical',
        reverse = false,
        duration = 0.8,
        ease = 'power3.out',
        initialOpacity = 0,
        animateOpacity = true,
        scale = 1,
        threshold = 0.1,
        delay = 0,
        disappearAfter = 0,
        disappearDuration = 0.5,
        disappearEase = 'power3.in',
        dotnet
    } = opts;

    const el = document.getElementById(elementId);
    if (!el) return;

    let scroller = null;

    if (container) {
        scroller = typeof container === 'string'
            ? document.querySelector(container)
            : container;
    } else {
        scroller = document.getElementById('snap-main-container') || null;
    }

    const axis = direction === 'horizontal' ? 'x' : 'y';
    const offset = reverse ? -distance : distance;
    const startPct = (1 - threshold) * 100;

    gsap.set(el, {
        [axis]: offset,
        scale,
        opacity: animateOpacity ? initialOpacity : 1,
        visibility: 'visible'
    });

    const tl = gsap.timeline({
        paused: true,
        delay,
        onComplete: () => {
            if (dotnet) dotnet.invokeMethodAsync('OnCompleteCallback');

            if (disappearAfter > 0) {
                gsap.to(el, {
                    [axis]: reverse ? distance : -distance,
                    scale: 0.8,
                    opacity: animateOpacity ? initialOpacity : 0,
                    delay: disappearAfter,
                    duration: disappearDuration,
                    ease: disappearEase,
                    onComplete: () => {
                        if (dotnet) dotnet.invokeMethodAsync('OnDisappearanceCompleteCallback');
                    }
                });
            }
        }
    });

    tl.to(el, {
        [axis]: 0,
        scale: 1,
        opacity: 1,
        duration,
        ease
    });

    if (isFixedOrSticky(el)) {
        tl.play();
        _instances[elementId] = { tl, st: null };
        return;
    }

    const st = ScrollTrigger.create({
        trigger: el,
        scroller,
        start: `top ${startPct}%`,
        once: true,
        onEnter: () => tl.play()
    });

    _instances[elementId] = { tl, st };
}

export function stop(elementId) {
    const inst = _instances[elementId];
    if (!inst) return;
    if (inst.st) inst.st.kill();
    inst.tl.kill();
    delete _instances[elementId];
}