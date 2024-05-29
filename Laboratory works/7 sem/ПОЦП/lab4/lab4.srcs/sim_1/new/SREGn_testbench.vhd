----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/06/2023 01:38:59 PM
-- Design Name: 
-- Module Name: SREGn_testbench - Behavioral
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

entity SREGn_testbench is
        Generic (n : integer := 4);
end SREGn_testbench;

architecture Behavioral of SREGn_testbench is

    function to_stdlogic(i: integer) return std_logic is
    begin
        case i is
            when 0 => return '0';
            when 1 => return '1';
            when others => return 'X';
        end case;
    end function;

    Component SREGn is 
    Generic (n : integer := 4);
    Port ( Sin : in STD_LOGIC;
           Sout : out STD_LOGIC_VECTOR (0 to n-1);
           SE : in STD_LOGIC;
           Clk : in STD_LOGIC;
           Rst : in STD_LOGIC);
    end component;
    
    Component SREGn_s is 
    Generic (n : integer := 4);
    Port ( Sin : in STD_LOGIC;
           Sout : out STD_LOGIC_VECTOR (0 to n-1);
           SE : in STD_LOGIC;
           Clk : in STD_LOGIC;
           Rst : in STD_LOGIC);
    end component;
    
    signal sin, se, clk, rst : std_logic;
    signal sout, bout : std_logic_vector(n-1 downto 0);
begin

    b: SREGn port map(Sin=>sin, Sout=>bout, SE=>se, Clk=>clk, Rst=>rst);
    s: SREGn_s port map(Sin=>sin, Sout=>sout, SE=>se, Clk=>clk, Rst=>rst);
    
    main: process
    begin 
        rst <= '1';
        wait for 20 ns;
        rst <= '0';
        
        for i in 0 to n-1 loop 
                for k in 0 to 1 loop
                    for d in 0 to 1 loop
                        se <= to_stdlogic(k);
                        sin <= to_stdlogic(d);
               
                        wait for 20 ns;
                        
                        assert (bout = sout)
                            report "testbench failed at num" & integer'image(i);
                    end loop;
                end loop;
        end loop;
        wait;
    end process;
    
    cl: process
    begin
        clk <= '0';
        wait for 10 ns;
        clk <= '1';
        wait for 10 ns;
    end process;
    
end Behavioral;
