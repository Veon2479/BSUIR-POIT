----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/02/2023 08:08:22 PM
-- Design Name: 
-- Module Name: RSLatch - Behavioral
-- Project Name: 
-- Target Devices: 
-- Tool Versions: 
-- Description: 
-- 
-- Dependencies: 
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
-- 
----------------------------------------------------------------------------------


library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity RSLatch is
    Port ( R : in STD_LOGIC;
           S : in STD_LOGIC;
           Q : out STD_LOGIC;
           NQ : out STD_LOGIC);
end RSLatch;

architecture Behavioral of RSLatch is
    Component Nor2 is 
        Port (
            pin1 : in std_logic;
            pin2 : in std_logic;
            pout : out std_logic
         );
    end component;
    
    signal s1, s2: std_logic;
begin

    nor2_1: Nor2 port map (pin1=>s2, pin2=>S, pout=>s1);
    nor2_2: Nor2 port map (pin1=>s1, pin2=>R, pout=>s2);
    
    Q <= s1;
    NQ <= s2;

end Behavioral;
